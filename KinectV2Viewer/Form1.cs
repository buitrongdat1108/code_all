
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Numerics;

using Microsoft.Kinect;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Kitware.VTK;

using VTKLibrary;

using PclSharp.Filters;
using PclSharp.IO;
using PclSharp.SampleConsensus;
using PclSharp.Segmentation;
using PclSharp.Std;
using PclSharp;

namespace KinectV2Viewer
{
    public partial class Form1 : Form
    {
        KinectSensor mySensor = null;
        MultiSourceFrameReader myReader = null;
        CoordinateMapper coordinateMapper = null;
        ColorSpacePoint[] colorSpacePoints;
        CameraSpacePoint[] cameraSpacePoints;
        public static int depthHeight = 0;
        public static int depthWidth = 0;
        public static int colorHeight = 0;
        public static int colorWidth = 0;
        public const double depthLimit = 3.0;
        public const double unitScale = 100.0;         // scale from m to cm etc.
        ushort[] depthFrameData = null;
        byte[] colorFrameData = null;

        bool kinectViewer = false;

        vtkPolyData scenePolyData = vtkPolyData.New();      
        vtkPoints points = vtkPoints.New();
        vtkRenderer Renderer = vtkRenderer.New();
        vtkRenderWindow RenderWindow = vtkRenderWindow.New();  
        vtkRenderWindowInteractor Iren = vtkRenderWindowInteractor.New();
        vtkInteractorStyleTrackballCamera style = vtkInteractorStyleTrackballCamera.New();
        vtkPLYReader reader = vtkPLYReader.New();
        vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
        vtkActor actor = vtkActor.New();

        List<vtkPolyData>  Poly=new List<vtkPolyData>();
        List<vtkVertexGlyphFilter> Gly = new List<vtkVertexGlyphFilter>();
        List<vtkMapper> Mapper = new List<vtkMapper>();
        List<vtkActor> Actor = new List<vtkActor>();
        List<vtkPoints> point1 = new List<vtkPoints>();
        List<PclSharp.Struct.PointXYZ> pointxyz = new List<PclSharp.Struct.PointXYZ>();
        List<PointCloudOfXYZ> pclOfXYZ = new List<PointCloudOfXYZ>();

        float[] td = new float[5];
        int tinhieuduong, tinhieuam;
        float[] SetupStep = new float[5];
        
        public Form1()
        {
            InitializeComponent();
            //Control.CheckForIllegalCrossThreadCalls = false;
            for(int i=0;i<5;i++)
            {
                td[i] = 0;
            }
            GetPortNames();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Geometry
            vtkVectorText text = vtkVectorText.New();
            text.SetText("Display 3D Point Clouds!");
            vtkElevationFilter elevation = vtkElevationFilter.New();       //??
            elevation.SetInputConnection(text.GetOutputPort());
            elevation.SetLowPoint(0, 0, 0);
            elevation.SetHighPoint(10, 0, 0);

            // mapper
            vtkPolyDataMapper textMapper = vtkPolyDataMapper.New();
            textMapper.SetInputConnection(elevation.GetOutputPort());

            // actor
            vtkActor textActor = vtkActor.New();
            textActor.SetMapper(textMapper);

            // get a reference to the renderwindow of our renderWindowControl1
            RenderWindow.SetParentId(pictureBoxPointCloud.Handle);
            RenderWindow.SetSize(pictureBoxPointCloud.Width, pictureBoxPointCloud.Height);

            // Setup the background gradient
            Renderer.GradientBackgroundOn();
            Renderer.SetBackground(0.5, 0.5, 1.0);
            Renderer.SetBackground2(0, 0, 0);

            // add actor to the renderer
            Renderer.AddActor(textActor);

            // ensure all actors are visible (in this example not necessarely needed,
            // but in case more than one actor needs to be shown it might be a good idea :))
            Renderer.ResetCamera();

            RenderWindow.AddRenderer(Renderer);
            RenderWindow.Render();

            Iren.SetRenderWindow(RenderWindow);
            Iren.SetInteractorStyle(style);
            Iren.Start();

            //Thread for handling serial communications, dùng để chạy song song đa tác vụ
            Thread updateKinect = new Thread(new ThreadStart(UpdateKinect));
            updateKinect.IsBackground = true;
            updateKinect.Start();
        }
        private void UpdateKinect()
        {
            mySensor = KinectSensor.GetDefault();
            if (mySensor != null)
            {
                mySensor.Open();
            }
            coordinateMapper = mySensor.CoordinateMapper;
            myReader = mySensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);// | FrameSourceTypes.Infrared | FrameSourceTypes.Body           

            depthWidth = mySensor.DepthFrameSource.FrameDescription.Width;             //chiều rộng của độ sâu được lấy từ camera kinect
            depthHeight = mySensor.DepthFrameSource.FrameDescription.Height;           // chiều cao của độ sâu được lấy từ camera kinect
            depthFrameData = new ushort[depthWidth * depthHeight];                     // kích thước của ảnh độ sâu = rộng x cao
            cameraSpacePoints = new CameraSpacePoint[depthWidth * depthHeight];
            colorSpacePoints = new ColorSpacePoint[depthWidth * depthHeight];

            colorWidth = mySensor.ColorFrameSource.FrameDescription.Width;
            colorHeight = mySensor.ColorFrameSource.FrameDescription.Height;
            colorFrameData = new byte[colorWidth * colorHeight * 32 / 8];

            myReader.MultiSourceFrameArrived += myReader_MultiSourceFrameArrived;
        }

        void myReader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            using (var colorFrame = reference.ColorFrameReference.AcquireFrame())
            using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
            {
                if (colorFrame != null && kinectViewer == true)
                {
                    //var bitmap = Extensions.ToBitmap(frame);
                    pictureBoxColor.Image = colorFrame.ToBitmap();
                }

                if (depthFrame != null && kinectViewer == true)
                {
                    //var bitmap = Extensions.ToBitmap(frame);
                    pictureBoxDepth.Image = depthFrame.ToBitmap();
                }

                if (colorFrame != null && depthFrame != null && kinectViewer == true)      // cái này để làm gì ??? 
                {
                    depthFrame.CopyFrameDataToArray(depthFrameData);
                    colorFrame.CopyConvertedFrameDataToArray(colorFrameData, ColorImageFormat.Bgra);
                    coordinateMapper.MapDepthFrameToColorSpace(depthFrameData, colorSpacePoints);
                    coordinateMapper.MapDepthFrameToCameraSpace(depthFrameData, cameraSpacePoints);
                }
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            kinectViewer = true;
            //UpdateKinect(); khởi động camera
            
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            //mục đích: tắt camera
            kinectViewer = false;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (myReader != null)        //nếu myReader là biến được kế thừa từ biến KinectSensor.mySensor, thì chẳng phải đóng hàm mySensor là đủ rồi à ???
            {
                myReader.Dispose();
            }

            if (mySensor != null)
            {
                mySensor.Close();
            }
        }
        private void getPointsCloud()   //hàm xử lý ảnh
        {
            vtkPoints pointss = vtkPoints.New();
            //scenePolyData = vtkPolyData.New();

            vtkUnsignedCharArray colors = vtkUnsignedCharArray.New();
            colors.SetNumberOfComponents(3);
            colors.SetName("Colors");

            for (int i = 0; i < cameraSpacePoints.Length; i++)
            {
                CameraSpacePoint p = cameraSpacePoints[i];
                Point3D pt = new Point3D();

                if (p.Z > depthLimit)
                {
                    continue;
                }

                else if (p.Z <= depthLimit)
                {
                    if (System.Single.IsNegativeInfinity(p.X) == true)
                    {
                        continue;
                    }

                    else if (System.Single.IsNegativeInfinity(p.X) == false)
                    {
                        ColorSpacePoint colPt = colorSpacePoints[i];

                        int colorX = (int)Math.Floor(colPt.X + 0.5);//30.2+0.5 = 30.7 --> 30; 30.6+0.5 = 31.1 --> 31       // cái này nghĩa là gì
                        int colorY = (int)Math.Floor(colPt.Y + 0.5);


                        if ((colorX >= 0) && (colorX < colorWidth) && (colorY >= 0) && (colorY < colorHeight))
                        {
                            int colorIndex = ((colorY * colorWidth) + colorX) * 32 / 8;  //cái này là gì 
                            Byte b = 0; Byte g = 0; Byte r = 0;

                            b = colorFrameData[colorIndex++];//b=colorFrameData[colorIndex]; colorIndex = colorIndex + 1;
                            g = colorFrameData[colorIndex++];
                            r = colorFrameData[colorIndex++];

                            System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);

                            pt.X = p.X * unitScale;
                            pt.Y = p.Z * unitScale;    // ??
                            pt.Z = p.Y * unitScale;    // ??

                            pointss.InsertNextPoint(pt.X, pt.Y, pt.Z);
                            colors.InsertNextTuple3(r, g, b);
                        }
                    }
                }
            }

            scenePolyData.SetPoints(pointss);
            scenePolyData.GetPointData().SetScalars(colors);

            vtkVertexGlyphFilter GlyphFilters = vtkVertexGlyphFilter.New();
            GlyphFilters.SetInput(scenePolyData);
            GlyphFilters.Update();

            scenePolyData.ShallowCopy(GlyphFilters.GetOutput());
        }

        
        private void buttonCapturePointCloud_Click(object sender, EventArgs e)
        {
         
            getPointsCloud();
            tabControl1.SelectedIndex = 1;
            if (scenePolyData.GetNumberOfPoints() > 0)
            {
                vtkVertexGlyphFilter GlyphFilter = vtkVertexGlyphFilter.New();   // để làm gì
                GlyphFilter.SetInput(scenePolyData);
                GlyphFilter.Update();

                vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
                mapper.SetInputConnection(GlyphFilter.GetOutputPort());

                vtkActor actor = vtkActor.New();
                actor.SetMapper(mapper);

                Renderer.RemoveAllViewProps();
                Renderer.AddActor(actor);
                Renderer.ResetCamera();

                RenderWindow.Render();
                
            }
        }

        private void buttonOpenPly_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "File Ply(*.ply)|*.ply|All File(*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.InitialDirectory = "C:\\Users\\DatDepTrai\\Desktop\\Models\\Models";
            tabControl1.SelectedIndex = 1;
            //openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                reader.SetFileName(openFileDialog.FileName);
                reader.Update();
                scenePolyData.ShallowCopy(reader.GetOutput());
                vtkVertexGlyphFilter gly = vtkVertexGlyphFilter.New();
                gly.SetInput(scenePolyData);
                gly.Update();

                mapper.SetInputConnection(gly.GetOutputPort());
                actor.SetMapper(mapper);
                Renderer.RemoveAllViewProps();
                Renderer.AddActor(actor);
                Renderer.SetBackground(0.1804, 0.5451, 0.3412);
                RenderWindow.AddRenderer(Renderer);
                RenderWindow.SetParentId(pictureBoxPointCloud.Handle);
                RenderWindow.SetSize(pictureBoxPointCloud.Width, pictureBoxPointCloud.Height);
                RenderWindow.Render();

                Iren.SetRenderWindow(RenderWindow);
                Iren.SetInteractorStyle(style);
                Iren.Start();
            }
        }



        public double DistancetoPlane(double[] o, double[] n, double[] point)
        {
            double distance = 0;
            distance = Math.Abs(n[0] * (o[0] - point[0]) + n[1] * (o[1] - point[1]) + n[2] * (o[2] - point[2]));

            return distance;
        }


        private void buttonTestPlaneDetection_Click(object sender, EventArgs e)
        {
            double[] p = new double[3];              //là tọa độ của pháp tuyến
            double[] o = new double[3];              // tọa độ của gốc
            vtkPolyData poly = vtkPolyData.New();
            vtkPolyData poly1 = vtkPolyData.New();
            vtkPoints output = vtkPoints.New();
            poly.ShallowCopy(/*reader.GetOutput()*/scenePolyData);
            VTKLibrary.Functions.planeDetection(poly, 0.25, 100, ref p, ref o);
            Console.WriteLine("p= " + p[0] + " ," + p[1] + ", " + p[2]);
            Console.WriteLine("o= " + o[0] + " ," + o[1] + " ," + o[2]);

            double[] pt = new double[3];
            for (int i = 0; i < poly.GetNumberOfPoints(); i++)
            {
                //nhận tọa độ các điểm từ ảnh
                pt = poly.GetPoint(i);
                //đưa vào hàm tính khoảng cách
                double distance = DistancetoPlane(o, p, pt);
                if (distance > 0.75)
                {
                    output.InsertNextPoint(pt[0], pt[1], pt[2]);
                }
            }
            Renderer.RemoveAllViewProps();
            poly1.SetPoints(output);
            points=output;
            scenePolyData.ShallowCopy(poly1);
            vtkVertexGlyphFilter gly1 = vtkVertexGlyphFilter.New();
            gly1.SetInput(poly1);
            gly1.Update();
            mapper.SetInputConnection(gly1.GetOutputPort());
            actor.SetMapper(mapper);
            actor.GetProperty().SetColor(1, 1, 1);
            Renderer.AddActor(actor);

            RenderWindow.Render();

        }
        double[] tdd = new double[4];
        private void ClusterExtraction_Click(object sender, EventArgs e)
        {
            //using (var cloud = new PointCloudOfXYZ())
            var cloud = new PointCloudOfXYZ();
            vtkPolyData polydata = vtkPolyData.New();
            polydata.ShallowCopy(scenePolyData/*reader.GetOutput()*/);
            double[] toado = new double[3];
            var pointXYZ = new PclSharp.Struct.PointXYZ();
            for (int j = 0; j <= polydata.GetNumberOfPoints(); j++)
            {
                toado = polydata.GetPoint(j);
                pointXYZ.X = (float)toado[0];
                pointXYZ.Y = (float)toado[1];
                pointXYZ.Z = (float)toado[2];
                cloud.Add(pointXYZ);
            }
            
            using (var clusterIndices = new VectorOfPointIndices())
            {
                

                using (var vg = new VoxelGridOfXYZ())
                {
                   // vg.SetInputCloud(cloud);
                    //vg.LeafSize = new Vector3(0.01f);

                    var cloudFiltered = new PointCloudOfXYZ();
                    // vg.filter(cloudFiltered);

                    cloudFiltered = cloud;
                    using (var seg = new SACSegmentationOfXYZ()
                    {
                        OptimizeCoefficients = true,
                        ModelType = SACModel.Plane,
                        MethodType = SACMethod.RANSAC,
                        MaxIterations = 1000,
                        DistanceThreshold = 0.75,//0.75;0.85
                    })
                    using (var cloudPlane = new PointCloudOfXYZ())
                    using (var coefficients = new PclSharp.Common.ModelCoefficients())// hệ số mẫu
                    using (var inliers = new PointIndices())// danh mục các điểm
                    {
                        
                        int i = 0;
                        int nrPoints = cloudFiltered.Points.Count;// nrPoints được gán số lượng các điểm pointcloud
                        
                        while (cloudFiltered.Points.Count > 0.3 * nrPoints)
                        {
                            seg.SetInputCloud(cloudFiltered);
                            seg.Segment(inliers, coefficients);
                            
                            if (inliers.Indices.Count == 0)
                                Assert.Fail("could not estimate a planar model for the given dataset");
                            
                            using (var extract = new ExtractIndicesOfXYZ() { Negative = false })//khai báo danh mục các phần ảnh
                            {
                                extract.SetInputCloud(cloudFiltered);// thiết lập các điểm pointcloud đưa vào extract
                                extract.SetIndices(inliers.Indices);
                                
                                extract.filter(cloudPlane);// cloudPlane là đầu ra
                                
                                extract.Negative = true;
                                var cloudF = new PointCloudOfXYZ();
                                extract.filter(cloudF);// cloudF là các điểm đầu ra
                                
                                cloudFiltered.Dispose();
                                cloudFiltered = cloudF;
                                
                            }

                            i++;
                        }
                        Console.WriteLine("pt mat phang = " + coefficients.Values[0] + "x + " + coefficients.Values[1] + "y + " + coefficients.Values[2] + "z + " + coefficients.Values[3]);
                        tdd[0] = coefficients.Values[0];
                        tdd[1] = coefficients.Values[1];
                        tdd[2] = coefficients.Values[2];
                        tdd[3] = coefficients.Values[3];
                        vtkPoints point = vtkPoints.New();
                        for (int k = 0; k <= cloudFiltered.Points.Count; k++)
                        {
                            point.InsertNextPoint(cloudFiltered.Points[k].X, cloudFiltered.Points[k].Y, cloudFiltered.Points[k].Z);

                        }
                        
                        Renderer.RemoveAllViewProps();
                        vtkPolyData poly = vtkPolyData.New();
                        poly.SetPoints(point);
                        vtkVertexGlyphFilter gly = vtkVertexGlyphFilter.New();
                        gly.SetInput(poly);
                        gly.Update();
                        mapper.SetInputConnection(gly.GetOutputPort());
                        actor.SetMapper(mapper);
                        actor.GetProperty().SetColor(1, 1, 1);
                        //Renderer.RemoveAllViewProps();
                        Renderer.AddActor(actor);
                        //RenderWindow.AddRenderer(Renderer);
                        RenderWindow.Render();

                        //Assert.IsTrue(i > 1, "Didn't find more than 1 plane");
                        var tree = new PclSharp.Search.KdTreeOfXYZ();
                        tree.SetInputCloud(cloudFiltered);

                        using (var ec = new EuclideanClusterExtractionOfXYZ
                        {
                            ClusterTolerance = 3.5,//3.5;4
                            MinClusterSize = /*450*/200,
                            MaxClusterSize = 25000,//25000,
                        })
                        {
                            ec.SetSearchMethod(tree);// dùng phương pháp tree
                            ec.SetInputCloud(cloudFiltered);// ec nhận giá trị các điểm cloudFiltered
                            ec.Extract(clusterIndices);// đưa kết quả ra clusterIndices
                        }
                        //khi đã phân đoạn được các vật thể bắt đầu tách ra
                        var Cluster = new List<PointCloudOfXYZ>();
                        foreach (var pis in clusterIndices)// pis là số lượng các vật thể, mỗi vật chứa 1 cụm điểm ảnh
                        {
                            //using (var cloudCluster = new PointCloudOfXYZ())// cloudCluster là các điểm ảnh trong từng vật thể
                            var cloudCluster = new PointCloudOfXYZ();
                            {
                                foreach (var pit in pis.Indices)// xét trong từng vật thể
                                    cloudCluster.Add(cloudFiltered.Points[pit]);

                                cloudCluster.Width = cloudCluster.Points.Count;
                                cloudCluster.Height = 1;
                                //Cluster.Add(cloudCluster);
                            }
                            Cluster.Add(cloudCluster);
                        }

                        var Cluster1 = new List<PointCloudOfXYZ>();
                        foreach (var pis1 in Cluster)
                        {
                            var pointcloudXYZ = new PointCloudOfXYZ();
                            pointcloudXYZ = pis1;
                            var pointcloudXYZ1 = new PointCloudOfXYZ();
                            var sor = new StatisticalOutlierRemovalOfXYZ();
                            sor.SetInputCloud(/*cloudFiltered*/pointcloudXYZ);
                            sor.MeanK = 50;
                            sor.StdDevMulThresh = 2.7;//2.7;3.5
                            sor.filter(pointcloudXYZ1);
                            Cluster1.Add(pointcloudXYZ1);
                            pclOfXYZ.Add(pointcloudXYZ1);
                        }


                        for (int k = 0; k < Cluster1.Count; k++)
                        {
                            vtkPoints poin = vtkPoints.New();
                            PclSharp.Std.Vector<PclSharp.Struct.PointXYZ> PointXYZ;
                            PointXYZ = Cluster1[k].Points;
                            for (int h = 0; h < PointXYZ.Count; h++)
                            {
                                poin.InsertNextPoint(PointXYZ[h].X, PointXYZ[h].Y, PointXYZ[h].Z);
                            }
                            point1.Add(poin);

                        }
                    
                        Renderer.RemoveAllViewProps();
                        Console.WriteLine("so vat phat hien dc =" + point1.Count);
                        for (int m = 0; m < point1.Count; m++)
                        {
                            vtkPolyData Poly1 = vtkPolyData.New();
                            vtkVertexGlyphFilter Gly1 = vtkVertexGlyphFilter.New();
                            vtkPolyDataMapper Mapper1 = vtkPolyDataMapper.New();
                            vtkActor Actor1 = vtkActor.New();
                            Poly1.SetPoints(point1[m]);
                            Gly1.SetInput(Poly1);
                            Gly1.Update();
                            Mapper1.SetInputConnection(Gly1.GetOutputPort());
                            Actor1.SetMapper(Mapper1);
                            if (m == 0)
                            {
                                Actor1.GetProperty().SetColor(1.0, 0.0, 0.0);
                            }
                            if (m == 1)
                            {
                                Actor1.GetProperty().SetColor(1.0, 0.5, 0.0);
                            }
                            if (m == 2)
                            {
                                Actor1.GetProperty().SetColor(1.0, 0.5, 0.5);
                            }
                            if (m == 3)
                            {
                                Actor1.GetProperty().SetColor(0.0, 1.0, 0.0);
                            }
                            if (m == 4)
                            {
                                Actor1.GetProperty().SetColor(0.0, 1.0, 0.5);
                            }
                            if (m == 6)
                            {
                                Actor1.GetProperty().SetColor(0.5, 1.0, 0.5);
                            }
                            if (m == 7)
                            {
                                Actor1.GetProperty().SetColor(0.0, 0.0, 1.0);
                            }
                            if (m == 8)
                            {
                                Actor1.GetProperty().SetColor(0.5, 0.0, 1.0);
                            }
                            if (m == 9)
                            {
                                Actor1.GetProperty().SetColor(0.5, 0.5, 0.5);
                            }
                            if (m == 10)
                            {
                                Actor1.GetProperty().SetColor(0.1, 0.1, 0.1);
                            }
                            if (m == 11)
                            {
                                Actor1.GetProperty().SetColor(0.2, 0.2, 0.2);
                            }
                            if (m == 12)
                            {
                                Actor1.GetProperty().SetColor(0.3, 0.3, 0.3);
                            }
                            if (m == 13)
                            {
                                Actor1.GetProperty().SetColor(0.4, 0.4, 0.4);
                            }
                            if (m == 14)
                            {
                                Actor1.GetProperty().SetColor(0.6, 0.6, 0.6);
                            }
                            if (m == 15)
                            {
                                Actor1.GetProperty().SetColor(0.7, 0.7, 0.7);
                            }
                            if (m == 16)
                            {
                                Actor1.GetProperty().SetColor(0.8, 0.8, 0.8);
                            }
                            if (m == 17)
                            {
                                Actor1.GetProperty().SetColor(0.9, 0.9, 0.9);
                            }
                            if (m == 18)
                            {
                                Actor1.GetProperty().SetColor(1.0, 1.0, 0.0);
                            }
                            if (m == 19)
                            {
                                Actor1.GetProperty().SetColor(1.0, 0.0, 1.0);
                            }
                            if (m == 20)
                            {
                                Actor1.GetProperty().SetColor(0.0, 1.0, 1.0);
                            }
                            if (m == 21)
                            {
                                Actor1.GetProperty().SetColor(1.0, 0.7, 0.4);
                            }
                            if (m > 20)
                            {
                                Actor1.GetProperty().SetColor(m * 1.0 / point1.Count, 1 - m * 1.0 / point1.Count, 0.0);
                            }

                            //Actor1.GetProperty().SetColor(m * 1.0 / point1.Count, 1 - m * 1.0 / point1.Count, 0.0);
                            Renderer.AddActor(Actor1);
                            
                            Poly.Add(Poly1);
                            Gly.Add(Gly1);
                            Mapper.Add(Mapper1);
                            Actor.Add(Actor1);

                        }
                        RenderWindow.Render();
                    }
                }
            }
        }

        private void OrientedBoundingBox_Click(object sender, EventArgs e)
        {
            vtkPoints pts = vtkPoints.New();
            
            for (int i = 0; i < Poly.Count; i++)
            {
                pts = Poly[i].GetPoints();
                //PLD = Poly[i];
                if (pts.GetNumberOfPoints() < 20) return;

                double[] Dcorner = new double[3];
                double[] Dmax = new double[3];
                double[] Dmin = new double[3];
                double[] Dmid = new double[3];
                double[] Dsize = new double[3];

                IntPtr corner = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * 3);
                IntPtr max = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * 3);
                IntPtr min = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * 3);
                IntPtr mid = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * 3);
                IntPtr size = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * 3);

                //IntPtr[] td = new IntPtr[3];
                vtkOBBTree OBB = vtkOBBTree.New();
                vtkOBBTree.ComputeOBB(pts, corner, max, min, mid, size);
                Marshal.Copy(corner, Dcorner, 0, 3);
                Marshal.Copy(max, Dmax, 0, 3);
                Marshal.Copy(min, Dmin, 0, 3);
                Marshal.Copy(mid, Dmid, 0, 3);
                Marshal.Copy(size, Dsize, 0, 3);

                //Console.WriteLine("Corner: " + Dcorner[0] + "," + Dcorner[1] + "," + Dcorner[2]);
                //Console.WriteLine("max: " + Dmax[0] + "," + Dmax[1] + "," + Dmax[2]);
                //Console.WriteLine("mid: " + Dmid[0] + "," + Dmid[1] + "," + Dmid[2]);
                //Console.WriteLine("min: " + Dmin[0] + "," + Dmin[1] + "," + Dmin[2]);
                //Console.WriteLine("size: " + Dsize[0] + "," + Dsize[1] + "," + Dsize[2]);

                vtkPoints pts1 = vtkPoints.New();
                vtkPolyLine PolyLine = vtkPolyLine.New();
                vtkCellArray cell = new vtkCellArray();
                vtkPolyDataMapper mapper1 = vtkPolyDataMapper.New();
                vtkActor actor2 = new vtkActor();
                vtkPolyData PLD = new vtkPolyData();
                
                
                double[,] p = new double[,]{
                    {Dcorner[0], Dcorner[1], Dcorner[2]},
                    {Dcorner[0] + Dmid[0], Dcorner[1] + Dmid[1], Dcorner[2] + Dmid[2]},
                    {Dcorner[0] + Dmid[0] + Dmin[0], Dcorner[1] + Dmid[1] + Dmin[1], Dcorner[2] + Dmid[2] + Dmin[2]},
                    {Dcorner[0] + Dmin[0], Dcorner[1] + Dmin[1], Dcorner[2] + Dmin[2]},
                    {Dcorner[0] + Dmax[0] + Dmin[0], Dcorner[1] + Dmax[1] + Dmin[1], Dcorner[2] + Dmax[2] + Dmin[2]},
                    {Dcorner[0] + Dmax[0], Dcorner[1] + Dmax[1], Dcorner[2] + Dmax[2]},
                    {Dcorner[0] + Dmid[0] + Dmax[0], Dcorner[1] + Dmax[1] + Dmid[1], Dcorner[2] + Dmax[2] + Dmid[2]},
                    {Dcorner[0] + Dmid[0] + Dmin[0] + Dmax[0], Dcorner[1] + Dmid[1] + Dmin[1] + Dmax[1], Dcorner[2] + Dmid[2] + Dmin[2] + Dmax[2]},
                    {Dcorner[0] + Dmax[0] + Dmin[0], Dcorner[1] + Dmax[1] + Dmin[1], Dcorner[2] + Dmax[2] + Dmin[2]},
                    {Dcorner[0] + Dmax[0], Dcorner[1] + Dmax[1], Dcorner[2] + Dmax[2]},
                    {Dcorner[0], Dcorner[1], Dcorner[2]},
                    {Dcorner[0] + Dmid[0], Dcorner[1] + Dmid[1], Dcorner[2] + Dmid[2]},
                    {Dcorner[0] + Dmid[0] + Dmax[0], Dcorner[1] + Dmax[1] + Dmid[1], Dcorner[2] + Dmax[2] + Dmid[2]},
                    {Dcorner[0] + Dmid[0] + Dmin[0] + Dmax[0], Dcorner[1] + Dmid[1] + Dmin[1] + Dmax[1], Dcorner[2] + Dmid[2] + Dmin[2] + Dmax[2]},
                    {Dcorner[0] + Dmid[0] + Dmin[0], Dcorner[1] + Dmid[1] + Dmin[1], Dcorner[2] + Dmid[2] + Dmin[2]},
                    {Dcorner[0] + Dmin[0], Dcorner[1] + Dmin[1], Dcorner[2] + Dmin[2]},
                    {Dcorner[0], Dcorner[1], Dcorner[2]},
                };
                for(int j = 0; j < 17; j++)
                    pts1.InsertNextPoint(p[j,0], p[j, 1], p[j, 2]);
                PolyLine.GetPointIds().SetNumberOfIds(17);
                for (int k = 0; k < 17; k++)
                {
                    PolyLine.GetPointIds().SetId(k, k);
                }
                cell.InsertNextCell(PolyLine);
                PLD.SetPoints(pts1);
                PLD.SetLines(cell);
                mapper1.SetInput(PLD);
                actor2.SetMapper(mapper1);
                actor2.GetProperty().SetColor(0.8, 0.8, 0.8);
                actor2.GetProperty().SetLineWidth(2);
                Renderer.AddActor(actor2);
                // tính độ dài các cạnh của đường bao
                double disMax = Math.Sqrt(Dmax[0] * Dmax[0] + Dmax[1] * Dmax[1] + Dmax[2] * Dmax[2]);
                double disMin = Math.Sqrt(Dmin[0] * Dmin[0] + Dmin[1] * Dmin[1] + Dmin[2] * Dmin[2]);
                double disMid = Math.Sqrt(Dmid[0] * Dmid[0] + Dmid[1] * Dmid[1] + Dmid[2] * Dmid[2]);
                Console.WriteLine("do dai canh max = " + disMax);
                Console.WriteLine("do dai canh min = " + disMin);
                Console.WriteLine("do dai canh midx = " + disMid);

                // tính góc
                double[,] u = new double[,] { { Dmax[0], Dmax[1] }, { 1, 0 }, { Dmin[0], Dmin[1] }, { 0, 1 } };
                double cornerx = (u[0, 0] * u[1, 0] + u[0, 1] * u[1, 1]) / (Math.Sqrt(u[0, 0] * u[0, 0] + u[0, 1] * u[0, 1]) * Math.Sqrt(u[1, 0] * u[1, 0] + u[1, 1] * u[1, 1]));
                double acx = Math.Acos(cornerx)*180/Math.PI;
                double cornery = (u[0, 0] * u[3, 0] + u[0, 1] * u[3, 1]) / (Math.Sqrt(u[0, 0] * u[0, 0] + u[0, 1] * u[0, 1]) * Math.Sqrt(u[3, 0] * u[3, 0] + u[3, 1] * u[3, 1]));
                double acy = Math.Acos(cornery) * 180 / Math.PI;
                double cornerx1 = (u[2, 0] * u[1, 0] + u[2, 1] * u[1, 1]) / (Math.Sqrt(u[2, 0] * u[2, 0] + u[2, 1] * u[2, 1]) * Math.Sqrt(u[1, 0] * u[1, 0] + u[1, 1] * u[1, 1]));
                double acx1 = Math.Acos(cornerx1) * 180 / Math.PI;
                double cornery1 = (u[2, 0] * u[3, 0] + u[3, 1] * u[2, 1]) / (Math.Sqrt(u[2, 0] * u[2, 0] + u[2, 1] * u[2, 1]) * Math.Sqrt(u[3, 0] * u[3, 0] + u[3, 1] * u[3, 1]));
                double acy1 = Math.Acos(cornery1) * 180 / Math.PI;
                Console.WriteLine("acx= " + acx + " acy= " + acy + " acx1= " + acx1 + " acy1= " + acy1);

                //tính tọa độ của tâm
                double[] t = new double[3];
                t[0] = Dcorner[0] + (Dmax[0] + Dmin[0] + Dmid[0])/2;
                t[1] = Dcorner[1] + (Dmax[1] + Dmin[1] + Dmid[1])/2;
                t[2] = Dcorner[2] + (Dmax[2] + Dmin[2] + Dmid[2])/2;
                Console.WriteLine("centre " + i + " =[" + t[0] + "," + t[1] + "," + t[2]);
                
                vtkPoints pointCenter = vtkPoints.New();
                vtkPolyLine PolyLine1 = vtkPolyLine.New();
                vtkCellArray cell3 = new vtkCellArray();
                vtkPolyDataMapper mapper3 = vtkPolyDataMapper.New();
                vtkActor actor3 = new vtkActor();
                vtkPolyData PLD3 = new vtkPolyData();
                double[,] htd = new double[,] { 
                    { t[0], t[1], t[2] }, 
                    { t[0] + 15, t[1], t[2] },
                    { t[0], t[1], t[2] },
                    { t[0], t[1]+15, t[2] },
                    { t[0], t[1], t[2] },
                    { t[0], t[1], t[2]+15 },
                    { t[0], t[1], t[2] }
                };
                for(int a=0;a<7;a++)
                {
                    pointCenter.InsertNextPoint(htd[a, 0], htd[a, 1], htd[a, 2]);
                }
                
                PolyLine1.GetPointIds().SetNumberOfIds(7);
                for(int b=0;b<7;b++)
                {
                    PolyLine1.GetPointIds().SetId(b, b);
                }
                cell3.InsertNextCell(PolyLine1);
                PLD3.SetPoints(pointCenter);
                PLD3.SetLines(cell3);
                mapper3.SetInput(PLD3);
                actor3.SetMapper(mapper3);
                actor3.GetProperty().SetColor(1, 1, 1);
                actor3.GetProperty().SetLineWidth(2);
                Renderer.AddActor(actor3);
            }
            RenderWindow.Render();
        }

        vtkUnsignedCharArray color = vtkUnsignedCharArray.New();
        
        private void Cut_Click(object sender, EventArgs e)
        {
            vtkPolyData receiveImageTo = vtkPolyData.New();
            vtkPolyData receiveImageTo1 = vtkPolyData.New();
            receiveImageTo.ShallowCopy(scenePolyData);
            vtkPoints convertPLDtoP = vtkPoints.New();
            vtkPoints limRange = vtkPoints.New();
            convertPLDtoP=receiveImageTo.GetPoints();
            double[] a,b,c,d = new double[3];
            b = convertPLDtoP.GetPoint(0);
            c = convertPLDtoP.GetPoint(0);
            color.SetNumberOfComponents(3);
            vtkDataArray SaveColor = receiveImageTo.GetPointData().GetScalars();
            
            for (int i=0;i< receiveImageTo.GetNumberOfPoints();i++)
            {
                a = receiveImageTo.GetPoint(i);
                d = SaveColor.GetTuple3(i);                         //3 color
                if (a[0] >= b[0]) { b[0] = a[0]; }
                if (a[1] >= b[1]) { b[1] = a[1]; }
                if (a[2] >= b[2]) { b[2] = a[2]; }
                if (c[0] >= a[0]) { c[0] = a[0]; }
                if (c[1] >= a[1]) { c[1] = a[1]; }
                if (c[2] >= a[2]) { c[2] = a[2]; }
                if ((a[0] > -20 && a[0] < 33) && (a[1] > 56.3 && a[1] < /*70*/68.2) && (a[2] > -5 && a[2] < 30)/*(a[0] > -20 && a[0] < 30) && (a[1] > 30 && a[1] < 71) && (a[2] > -5 && a[2] < 30)*//*(a[0] > 70 && a[0] < 510) && (a[1] > 70 && a[1] < 423) && (a[2] < 900 && a[2] > 400)*/)//x ngnag, y doc, z dung
                {
                    limRange.InsertNextPoint(a[0], a[1], a[2]);
                    color.InsertNextTuple3(d[0], d[1], d[2]);
                }
            }
            
            vtkVertexGlyphFilter Gly1 = new vtkVertexGlyphFilter();
            vtkPolyDataMapper Mapper1 = vtkPolyDataMapper.New();
            vtkActor Actor1 = new vtkActor();
            receiveImageTo1.SetPoints(limRange);
            receiveImageTo1.GetPointData().SetScalars(color);
            scenePolyData.DeleteCells();
            scenePolyData.ShallowCopy(receiveImageTo1);
            Gly1.SetInput(receiveImageTo1); 
            Gly1.Update();
            Mapper1.SetInputConnection(Gly1.GetOutputPort());
            Actor1.SetMapper(Mapper1);
            Renderer.RemoveAllViewProps();
            Renderer.AddActor(Actor1);
            RenderWindow.Render();
        }

        private void buttonSavePLY_Click(object sender, EventArgs e)
        {
            vtkPLYWriter savePLY = vtkPLYWriter.New();
            savePLY.SetInput(scenePolyData);
            string path = DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + ".ply";
            saveFileDialog1.InitialDirectory = "C:\\Users\\Chien PC\\Desktop\\Models\\Models";
            saveFileDialog1.FileName = path;
            //saveFileDialog1.Filter= "File Ply(*.ply)|*.ply|All File(*.*)|*.*";
            //saveFileDialog1.FilterIndex = 2;
            //saveFileDialog1.DefaultExt = "ply";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                savePLY.SetFileName(saveFileDialog1.FileName);
                savePLY.SetArrayName("Colors");
                savePLY.Write();
            }
        }

        private void buttonRemovingOutliers_Click(object sender, EventArgs e)
        {
            vtkPolyData PLData = vtkPolyData.New();
            PLData.ShallowCopy(scenePolyData);
            var pointXYZ = new PclSharp.Struct.PointXYZ();
            double[] cooPoint = new double[3];
            var pointcloudXYZ = new PointCloudOfXYZ();
            var pointcloudXYZ1 = new PointCloudOfXYZ();
            for (int i=0; i<=PLData.GetNumberOfPoints();i++)
            {
                cooPoint = PLData.GetPoint(i);
                pointXYZ.X = (float)cooPoint[0];
                pointXYZ.Y = (float)cooPoint[1];
                pointXYZ.Z = (float)cooPoint[2];
                pointcloudXYZ.Add(pointXYZ);
            }
            var sor=new StatisticalOutlierRemovalOfXYZ();
            sor.SetInputCloud(pointcloudXYZ);
            sor.MeanK=50;
            sor.StdDevMulThresh = 1.5;
            sor.filter(pointcloudXYZ1);

            vtkPoints point = vtkPoints.New();
            for (int k = 0; k <= pointcloudXYZ1.Points.Count; k++)
            {
                point.InsertNextPoint(pointcloudXYZ1.Points[k].X, pointcloudXYZ1.Points[k].Y, pointcloudXYZ1.Points[k].Z);

            }
            Renderer.RemoveAllViewProps();
            vtkPolyData poly = vtkPolyData.New();
            poly.SetPoints(point);
            scenePolyData.ShallowCopy(poly);
            vtkVertexGlyphFilter gly = vtkVertexGlyphFilter.New();
            gly.SetInput(poly);
            gly.Update();
            mapper.SetInputConnection(gly.GetOutputPort());
            actor.SetMapper(mapper);
            actor.GetProperty().SetColor(1, 1, 1);
            //Renderer.RemoveAllViewProps();
            Renderer.AddActor(actor);
            //RenderWindow.AddRenderer(Renderer);
            RenderWindow.Render();
        }

        List<double> phi = new List<double>();
        List<double> endpoint = new List<double>();
        //List<double> addmatAngleRo = new List<double>();
        
        private void buttonAABB_Click(object sender, EventArgs e)
        {
            Renderer.RemoveAllViewProps();
            double[] addst = new double[899];
            
            double[] a, b, c, hmax = new double[3];
            //double[] color2 = new double[3];
            //vtkUnsignedCharArray color1 = vtkUnsignedCharArray.New();
            int color1=0;
            foreach (var poly in Poly)
            {
                color1++;
                vtkPolyData PolyYToZ = vtkPolyData.New();
                vtkPoints PointYToZ = vtkPoints.New();
                vtkVertexGlyphFilter glyYToZ = vtkVertexGlyphFilter.New();
                vtkPolyDataMapper mapperYToZ = vtkPolyDataMapper.New();
                vtkActor actYToZ = vtkActor.New();
                //color1.SetNumberOfComponents(3);
                //vtkDataArray SaveColor = poly.GetPointData().GetScalars();
                double[] pointIn = new double[3];
                double[] pointYToZ = new double[3];
                for (int i = 0; i < poly.GetNumberOfPoints(); i++)
                {
                    pointIn = poly.GetPoint(i);
                    //color2 = SaveColor.GetTuple3(i);
                    pointYToZ[0] = pointIn[0];
                    pointYToZ[1] = pointIn[2];
                    pointYToZ[2] = pointIn[1];
                    PointYToZ.InsertNextPoint(pointYToZ[0], pointYToZ[1], pointYToZ[2]);
                    //color1.InsertNextTuple3(color2[0], color2[1], color2[2]);
                }
                PolyYToZ.SetPoints(PointYToZ);
                //PolyYToZ.GetPointData().SetScalars(color1);
                glyYToZ.SetInput(PolyYToZ);
                
                mapperYToZ.SetInputConnection(glyYToZ.GetOutputPort());
                actYToZ.SetMapper(mapperYToZ);
                actYToZ.GetProperty().SetColor(1 - 0.2 * (color1 - 1), 0.1 * (color1 - 1), 0.2 * (color1 - 1));
                Renderer.AddActor(actYToZ);
                RenderWindow.Render();

                vtkPoints point = vtkPoints.New();
                vtkPolyData polydata = vtkPolyData.New();
                vtkPolyData polydata1 = vtkPolyData.New();
                vtkActor act1 = vtkActor.New();
                vtkActor act2 = vtkActor.New();
                vtkPolyDataMapper vmapper1 = vtkPolyDataMapper.New();
                vtkPolyDataMapper vmapper2 = vtkPolyDataMapper.New();
                b = PolyYToZ.GetPoint(0)/*poly.GetPoint(0)*/;
                c = PolyYToZ.GetPoint(0)/*poly.GetPoint(0)*/;
                for (int i = 0; i < /*poly*/PolyYToZ.GetNumberOfPoints(); i++)
                {
                    a = /*poly*/PolyYToZ.GetPoint(i);
                    if (a[0] >= b[0]) { b[0] = a[0]; }  // xmax
                    if (a[1] >= b[1]) { b[1] = a[1]; }  // ymax
                    if (a[2] >= b[2]) { b[2] = a[2]; }  // zmax
                    if (c[0] >= a[0]) { c[0] = a[0]; }  // xmin
                    if (c[1] >= a[1]) { c[1] = a[1]; }  // ymin
                    if (c[2] >= a[2]) { c[2] = a[2]; hmax[0] = a[0]; hmax[1] = a[2]; hmax[2] = a[1]; }  // zmin
                }
                double kc = Math.Abs(hmax[0] * tdd[0] + hmax[1] * tdd[1] + hmax[2] * tdd[2] + tdd[3]) / Math.Sqrt(tdd[0]*tdd[0]+ tdd[1] * tdd[1] + tdd[2] * tdd[2]);
                Console.WriteLine("chieu cao cua vat la: " + kc);
                //Renderer.RemoveAllViewProps();
                vtkPlane plane = vtkPlane.New();
                plane.SetOrigin(0.0, /*c[1]*/0.0, /*0.0*/c[2]);
                plane.SetNormal(0.0, /*1.0*/0.0, 1.0);
                double[] p = new double[3];
                double[] projected = new double[3];
                for (int i = 0; i < /*poly*/PolyYToZ.GetNumberOfPoints(); i++)
                {
                    p = /*poly*/PolyYToZ.GetPoint(i);

                    IntPtr pP = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * 3);
                    IntPtr pProjected = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * 3);
                    Marshal.Copy(p, 0, pP, 3);
                    Marshal.Copy(projected, 0, pProjected, 3);

                    // NOTE: normal assumed to have magnitude 1
                    plane.ProjectPoint(pP, pProjected);
                    Marshal.Copy(pProjected, projected, 0, 3);
                    Marshal.FreeHGlobal(pP);
                    Marshal.FreeHGlobal(pProjected);
                    point.InsertNextPoint(projected[0], projected[1], projected[2]);
                }
                polydata.SetPoints(point);
                vtkPoints poi = vtkPoints.New();
                vtkPoints poi1 = vtkPoints.New();
                vtkPolyData polydat = vtkPolyData.New();
                vtkPolyData polydat2 = vtkPolyData.New();
                vtkCellArray cellarray1 = vtkCellArray.New();
                vtkCellArray cellarray2 = vtkCellArray.New();
                vtkPolyLine PolyLine = vtkPolyLine.New();
                vtkPolyLine PolyLine2 = vtkPolyLine.New();
                vtkRenderer Ren = vtkRenderer.New();
                double[,] saveArea = new double[899, 6];
                double[,] box = new double[,] {
                    {0,0,0 },
                    {10,0,0 },
                    {0,0,0 },
                    {0,10,0 },
                    {0,0,0 },
                    {0,0,10 },
                };
                //Console.WriteLine("b ");
                for (int k = 0; k < 6; k++)
                {
                    poi.InsertNextPoint(box[k, 0], box[k, 1], box[k, 2]);
                }
                
                PolyLine.GetPointIds().SetNumberOfIds(6);
                for (int u = 0; u < 6; u++)
                {
                    PolyLine.GetPointIds().SetId(u, u);
                }
                cellarray1.InsertNextCell(PolyLine);
                polydat.SetPoints(poi);
                polydat.SetLines(cellarray1);
                vmapper1.SetInput(polydat);
                act1.SetMapper(vmapper1);
                act1.GetProperty().SetColor(1, 1, 1);
                act1.GetProperty().SetLineWidth(2);
                Renderer.AddActor(act1);
                //RenderWindow.Render();
                //thuc hien phep quay quanh truc z
                for (int i = 0; i < 899; i++)
                {
                    vtkPoints poin = vtkPoints.New();
                    double[] t = new double[3];
                    double[] s = new double[3];
                    double dai, rong, dt;
                    for (int j = 0; j < polydata.GetNumberOfPoints(); j++)
                    {
                        
                        t = polydata.GetPoint(j);
                        Matrix4x4 mat = new Matrix4x4();
                        mat.M11 = (float)Math.Cos(2 * i * Math.PI / 3600);
                        mat.M12 = /*0*/-(float)Math.Sin(2 * i * Math.PI / 3600);
                        mat.M13 = 0/*(float)(Math.Sin(2 * i * Math.PI / 3600))*/;
                        mat.M14 = 0;
                        mat.M21 = /*0*/(float)Math.Sin(2 * i * Math.PI / 3600);
                        mat.M22 = /*1*/(float)Math.Cos(2 * i * Math.PI / 3600);
                        mat.M23 = 0;
                        mat.M24 = 0;
                        mat.M31 = 0/*-(float)Math.Sin(2 * i * Math.PI / 3600)*/;
                        mat.M32 = 0;
                        mat.M33 = 1/*(float)Math.Cos(2 * i * Math.PI / 3600)*/;
                        mat.M34 = 0;
                        mat.M41 = 0;
                        mat.M42 = 0;
                        mat.M43 = 0;
                        mat.M44 = 1;
                        Matrix4x4 matbd = new Matrix4x4((float)t[0], (float)t[1], (float)t[2], 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        Matrix4x4 mats = new Matrix4x4();
                        mats = matbd * mat;
                        s[0] = mats.M11;
                        s[1] = mats.M12;
                        s[2] = mats.M13;
                        poin.InsertNextPoint(s[0], s[1], s[2]);
                    }
                    polydata1.SetPoints(poin);
                    
                    double[] d, f, g = new double[3];
                    d = polydata1.GetPoint(0);
                    f = polydata1.GetPoint(0);
                    for (int h = 0; h < polydata1.GetNumberOfPoints(); h++)
                    {
                        g = polydata1.GetPoint(h);
                        if (g[0] >= d[0]) { d[0] = g[0]; }  // xmax
                        if (g[1] >= d[1]) { d[1] = g[1]; }  // ymax
                        if (g[2] >= d[2]) { d[2] = g[2]; }  // zmax
                        if (f[0] >= g[0]) { f[0] = g[0]; }  // xmin
                        if (f[1] >= g[1]) { f[1] = g[1]; }  // ymin
                        if (f[2] >= g[2]) { f[2] = g[2]; }  // zmin
                        saveArea[i, 0] = f[0];
                        saveArea[i, 1] = f[1];
                        saveArea[i, 2] = f[2];
                        saveArea[i, 3] = d[0];
                        saveArea[i, 4] = d[1];
                        saveArea[i, 5] = d[2];
                    }
                    
                    dai = Math.Sqrt((d[0] - f[0]) * (d[0] - f[0]));
                    rong = Math.Sqrt((d[1] - f[1]) * (d[1] - f[1]));
                    dt = dai * rong;
                    addst[i] = dt;
                    
                }
                double min = addst[0];
                int index=0;
                for (int i = 0; i < 899; i++)
                {
                    if (addst[i] <= min)
                    {
                        index = i;
                        min = addst[i];
                    }
                }
                double[,] box1 = new double[,] {
                        {saveArea[index, 0],saveArea[index, 1],saveArea[index, 2] },
                        {saveArea[index, 3],saveArea[index, 1],saveArea[index, 2] },
                        {saveArea[index, 3],saveArea[index, 4],saveArea[index, 2] },
                        {saveArea[index, 0],saveArea[index, 4],saveArea[index, 2] },
                        {saveArea[index, 0],saveArea[index, 1],saveArea[index, 2] },
                    };
                for (int k = 0; k < 5; k++)
                {
                    poi1.InsertNextPoint(box1[k, 0], box1[k, 1], box1[k, 2]);
                }
                double[] retur = new double[3];
                double[,] s1 = new double[5,3];
                
                vtkPoints poi2 = vtkPoints.New();
                for (int j = 0; j < poi1.GetNumberOfPoints(); j++)
                {
                    retur = poi1.GetPoint(j);
                    Matrix4x4 mat = new Matrix4x4();
                    mat.M11 = (float)Math.Cos(-2 * index * Math.PI / 3600);
                    mat.M12 = -(float)Math.Sin(-2 * index * Math.PI / 3600);
                    mat.M13 = 0;
                    mat.M14 = 0;
                    mat.M21 = (float)Math.Sin(-2 * index * Math.PI / 3600);
                    mat.M22 = (float)Math.Cos(-2 * index * Math.PI / 3600);
                    mat.M23 = 0;
                    mat.M24 = 0;
                    mat.M31 = 0;
                    mat.M32 = 0;
                    mat.M33 = 1;
                    mat.M34 = 0;
                    mat.M41 = 0;
                    mat.M42 = 0;
                    mat.M43 = 0;
                    mat.M44 = 1;
                    Matrix4x4 matsource = new Matrix4x4((float)retur[0], (float)retur[1], (float)retur[2], 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    Matrix4x4 matdestination = new Matrix4x4();
                    matdestination = matsource * mat;
                    s1[j,0] = matdestination.M11;
                    s1[j,1] = matdestination.M12;
                    s1[j,2] = matdestination.M13;
                }
                double[,] box2 = new double[,]{
                    {s1[0,0],s1[0,1],s1[0,2] },
                    {s1[1,0],s1[1,1],s1[1,2] },
                    {s1[2,0],s1[2,1],s1[2,2] },
                    {s1[3,0],s1[3,1],s1[3,2] },
                    {s1[4,0],s1[4,1],s1[4,2] },
                    {s1[4,0],s1[4,1],b[2] },
                    {s1[1,0],s1[1,1],b[2] },
                    {s1[1,0],s1[1,1],s1[1,2] },
                    {s1[1,0],s1[1,1],b[2] },
                    {s1[2,0],s1[2,1],b[2] },
                    {s1[2,0],s1[2,1],s1[2,2] },
                    {s1[2,0],s1[2,1],b[2] },
                    {s1[3,0],s1[3,1],b[2] },
                    {s1[3,0],s1[3,1],s1[3,2] },
                    {s1[3,0],s1[3,1],b[2] },
                    {s1[4,0],s1[4,1],b[2] },
                };
                for(int j=0;j<16;j++)
                {
                    poi2.InsertNextPoint(box2[j, 0], box2[j, 1], box2[j, 2]);
                }
                PolyLine2.GetPointIds().SetNumberOfIds(16);
                for (int u = 0; u < 16; u++)
                {
                    PolyLine2.GetPointIds().SetId(u, u);
                }
                cellarray2.InsertNextCell(PolyLine2);
                polydat2.SetPoints(poi2);
                polydat2.SetLines(cellarray2);
                vmapper2.SetInput(polydat2);
                act2.SetMapper(vmapper2);
                act2.GetProperty().SetColor(1.0, 1.0, 1.0);
                act2.GetProperty().SetLineWidth(2);
                Renderer.AddActor(act2);
                RenderWindow.Render();
                Vector3[] vector3 = new Vector3[3];
                vector3[0].X = (float)(s1[1, 0] - s1[0, 0]);
                vector3[0].Y = (float)(s1[1, 1] - s1[0, 1]);
                vector3[0].Z = (float)(s1[1, 2] - s1[0, 2]);
                vector3[1].X = (float)(s1[3, 0] - s1[0, 0]);
                vector3[1].Y = (float)(s1[3, 1] - s1[0, 1]);
                vector3[1].Z = (float)(s1[3, 2] - s1[0, 2]);
                vector3[2].X = (float)(s1[4, 0] - s1[0, 0]);
                vector3[2].Y = (float)(s1[4, 1] - s1[0, 1]);
                vector3[2].Z = (float)(b[2] - s1[0, 2]);
                double[] corner = new double[] { s1[0, 0], s1[0, 1]/*c[1]*/, s1[0, 2] };
                Vector3 vector3X = vector3[0];
                Vector3 vector3Y = vector3[1];
                Vector3 vector3Z = vector3[2];
                Vector3 vector3s = new Vector3();
                float max, mid, min1;
                for (int dem1 = 0; dem1 <3; dem1++)
                {
                    for (int de = dem1+1; de < 3; de++)
                    {
                        if (vector3[de].Length() < vector3[dem1].Length())
                        {
                            vector3s = vector3[dem1];
                            vector3[dem1] = vector3[de];
                            vector3[de] = vector3s;
                        }
                    }
                }
                max = vector3[2].Length();
                mid = vector3[1].Length();
                min1 = vector3[0].Length();
                Console.WriteLine("max= " + max + "; mid= " + mid + "; min= " + min1);
                double[] pc = new double[4];
                pc[0] = corner[0] + (vector3[0].X + vector3[1].X + vector3[2].X) / 2;
                pc[1] = corner[1] + (vector3[0].Y + vector3[1].Y + vector3[2].Y) / 2;
                pc[2] = corner[2] + (vector3[0].Z + vector3[1].Z + vector3[2].Z) / 2;
                pc[3] = 1;
                Console.WriteLine("center[" + pc[0] + "; " + pc[1] + "; " + pc[2] + "]");
                for(int add=0;add<4;add++)
                {
                    endpoint.Add(pc[add]);
                }
                // ma tran hieu chuan toa do diem
                
                
            }
            disposeCenter();
            tabControl2.SelectedIndex = 1;
        }

        double[] er = new double[4];
        double[,] matAngleRo = new double[3, 3];
        double[,] matAngleRo_T = new double[3, 3];
        
        private void disposeCenter()
        {
            double[,] epoint = new double[endpoint.Count / 4, 4];
            double[,] epointafter = new double[endpoint.Count / 4, 4];
            double[] ss=new double[4];
            for (int i=0;i<endpoint.Count/4;i++)
            {
                for(int j=0;j<4;j++)
                {
                    epoint[i, j] = endpoint[4 * i + j];
                }
            }
            for(int i=0;i<endpoint.Count/4;i++)
            {
                for(int j=i+1;j<endpoint.Count/4;j++)
                {
                    if(epoint[i,0]>epoint[j,0])
                    {
                        for(int h=0;h<4;h++)
                        {
                            ss[h] = epoint[j, h];
                            epoint[j, h] = epoint[i, h];
                            epoint[i, h] = ss[h];
                        }
                    }
                    if(epoint[i, 0] == epoint[j, 0])
                    {
                        if(epoint[i, 1] < epoint[j, 1])
                        {
                            for (int h = 0; h < 4; h++)
                            {
                                ss[h] = epoint[j, h];
                                epoint[j, h] = epoint[i, h];
                                epoint[i, h] = ss[h];
                            }
                        }
                    }
                }
            }
            Matrix4x4 calib = new Matrix4x4((float)0.9970, (float)0.0127, (float)0.0759, (float)-77.0964, (float)0.0088, (float)-0.9986, (float)0.0519, (float)447.8857, (float)0.0764, (float)-0.0511, (float)-0.9958, (float)675.0744, 0, 0, 0, 1);
            for(int k=0;k<endpoint.Count/4;k++)
            {
                er[0] = calib.M11 * epoint[k, 0] * 10 + calib.M12 * epoint[k, 1] * 10 + calib.M13 * epoint[k, 2] * 10 + calib.M14 * epoint[k, 3];
                er[1] = calib.M21 * epoint[k, 0] * 10 + calib.M22 * epoint[k, 1] * 10 + calib.M23 * epoint[k, 2] * 10 + calib.M24 * epoint[k, 3];
                er[2] = calib.M31 * epoint[k, 0] * 10 + calib.M32 * epoint[k, 1] * 10 + calib.M33 * epoint[k, 2] * 10 + calib.M34 * epoint[k, 3];
                er[3] = calib.M41 * epoint[k, 0] * 10 + calib.M42 * epoint[k, 1] * 10 + calib.M43 * epoint[k, 2] * 10 + calib.M44 * epoint[k, 3];
                Console.WriteLine("toa do diem cuoi vat " + k + " la "+ er[0] + ";" + er[1] + ";" + er[2] + ";" + er[3]);
                double angle = Math.Atan(er[1] / er[0]);
                Console.WriteLine("goc quay " + angle * 360 / (2 * Math.PI));
                matAngleRo[0, 0] = Math.Cos(angle); matAngleRo[0, 1] = 0; matAngleRo[0, 2] = Math.Sin(angle);
                matAngleRo[1, 0] = Math.Sin(angle); matAngleRo[1, 1] = 0; matAngleRo[1, 2] = -Math.Cos(angle);
                matAngleRo[2, 0] = 0; matAngleRo[2, 1] = 1; matAngleRo[2, 2] = 0;

                double[] R0E_d = new double[3];
                double[] dE = new double[] { 130, 0.0, 0.0 };
                for (int i = 0; i < 3; i++)
                {
                    double tg = 0, tg1 = 0;
                    int j;
                    for (j = 0; j < 3; j++)
                    {
                        tg = matAngleRo[i, j] * dE[j];
                        tg1 += tg;
                    }
                    R0E_d[i] = tg1;
                }

                double[] qC = new double[3];
                qC[0] = er[0] - R0E_d[0];
                qC[1] = er[1] - R0E_d[1];
                qC[2] = er[2] - R0E_d[2];

                double[] q = new double[5];
                double a1, b1, c1, d1, h1 = 234, h2 = 221, h3 = 128, h4 = 96, h5 = 130;
                q[0] = Math.Atan2(qC[1], qC[0]);
                a1 = -2 * h2 * (qC[0] * Math.Cos(q[0]) + qC[1] * Math.Sin(q[0]));
                b1 = 2 * h2 * (h1 - qC[2]);
                c1 = (h3 + h4) * (h3 + h4) - ((qC[0] * Math.Cos(q[0]) + qC[1] * Math.Sin(q[0])) * (qC[0] * Math.Cos(q[0]) + qC[1] * Math.Sin(q[0])) + h1 * h1 - 2 * h1 * qC[2] + h2 * h2 + qC[2] * qC[2]);
                d1 = Math.Atan2(b1 / Math.Sqrt(a1 * a1 + b1 * b1), a1 / Math.Sqrt(a1 * a1 + b1 * b1));
                q[1] = d1 - Math.Atan2(Math.Sqrt(1 - (c1 / (Math.Sqrt(a1 * a1 + b1 * b1)) * (c1 / Math.Sqrt(a1 * a1 + b1 * b1)))), c1 / Math.Sqrt(a1 * a1 + b1 * b1));
                q[2] = Math.Atan2((qC[2] - h1 - h2 * Math.Sin(q[1])), (qC[0] * Math.Cos(q[0]) + qC[1] * Math.Sin(q[0]) - h2 * Math.Cos(q[1]))) - q[1];

                q[3] = Math.Asin(-Math.Cos(q[0]) * Math.Sin(q[1] + q[2]) * matAngleRo[0, 2] - Math.Sin(q[0]) * Math.Sin(q[1] + q[2]) * matAngleRo[1, 2] + Math.Cos(q[1] + q[2]) * matAngleRo[2, 2]);
                q[4] = Math.PI - Math.Asin(Math.Cos(q[0]) * Math.Cos(q[1] + q[2]) * matAngleRo[0, 0] + Math.Sin(q[0]) * Math.Cos(q[1] + q[2]) * matAngleRo[1, 0] + Math.Sin(q[1] + q[2]) * matAngleRo[2, 0]);

                q[0] = -(Math.PI / 2 - q[0]);
                q[1] = -(Math.PI / 2 - q[1]);
                if (q[3] < 0.001)
                {
                    q[3] = 0;
                }
                q[4] = q[4] - Math.PI / 2;
                Console.WriteLine("q[0] = " + q[0] * 180 / (Math.PI) + ", q[1] = " + q[1] * 180 / (Math.PI) + ", q[2] = " + q[2] * 180 / (Math.PI) + ", q[3] = " + q[3] * 180 / (Math.PI) + ", q[4] = " + q[4] * 180 / (Math.PI));

                phi.Add(((q[0] * 180 / (Math.PI))+0.5) / 0.4136);
                phi.Add(((q[1] * 180 / (Math.PI))+1) / 0.421);
                phi.Add(((q[2] * 180 / (Math.PI))-2) / 0.8004);
                phi.Add(q[3] * 180 / (Math.PI) / 4.05);
                phi.Add(((q[4] * 180 / (Math.PI)+1.5)) / 0.9944);
                Console.WriteLine("Y" + q[0] * 180 / (Math.PI) / 0.4136 + " Z" + q[1] * 180 / (Math.PI) / 0.421 + " X" + q[2] * 180 / (Math.PI) / 0.8004 + " E" + q[3] * 180 / (Math.PI) / 4.05 + " H" + q[4] * 180 / (Math.PI) / 0.9944);
                
            }
        }

        private void ClearPointCloud_Click(object sender, EventArgs e)
        {

            scenePolyData.RemoveDeletedCells();
        }

        private void GetPortNames()
        {
            string[] portNames = SerialPort.GetPortNames(); //load all names of  com ports to string 
            comboBoxPort.Items.Clear();                     //delete previous names in combobox items 
            foreach (string s in portNames)                 //add this names to comboboxPort items
            {
                comboBoxPort.Items.Add(s);
            }
            if (comboBoxPort.Items.Count > 0)   //if there are some com ports ,select first 
            {
                comboBoxPort.SelectedIndex = 0;
            }
            else
            {
                comboBoxPort.Text = "No COM Port "; //if there are no com ports ,write No Com Port
            }
        }

        private void buttonInitialPort_Click(object sender, EventArgs e)
        {
            try
            {
                //make sure port is not open
                if (!serialPort1.IsOpen)
                {

                    serialPort1.PortName = comboBoxPort.Text;
                    serialPort1.BaudRate = int.Parse(comboBoxBaudRate.Text);
                    serialPort1.DataReceived += serialPort1_DataReceived;
                    serialPort1.Open();
                    labelPortStatus.Text = "Connected";
                    labelPortStatus.BackColor = Color.Green;
                    listBoxReceived.Items.Clear();
                }

                else
                {
                    serialPort1.Close();
                    labelPortStatus.Text = "Port is not opened!";
                    labelPortStatus.BackColor = Color.Red;
                    listBoxReceived.Items.Clear();
                }
            }
            catch
            {
                labelPortStatus.Text = "Unauthorized Access";
                labelPortStatus.BackColor = Color.Yellow;
            }
        }
        delegate void deleDataReceived();
        void DataReceived()
        {
            if (listBoxReceived.InvokeRequired)
            {
                this.Invoke(new deleDataReceived(DataReceived));
            }
            else
            {

                if (serialPort1.IsOpen)
                {
                    string received = String.Empty;
                    try
                    {
                        received = serialPort1.ReadExisting();//ReadLine() ReadExisting

                    }
                    catch (TimeoutException)
                    {
                        received = "Timeout Exception";
                    }
                    listBoxReceived.Items.Add(received);
                }
            }
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread DataReceivedthread = new Thread(DataReceived);
            DataReceivedthread.Start();
            //save the input from the Arduino
/*
            if (serialPort1.IsOpen)
            {
                string received = String.Empty;
                try
                {
                    received = serialPort1.ReadExisting();//ReadLine() ReadExisting

                }
                catch (TimeoutException)
                {
                    received = "Timeout Exception";
                }
                listBoxReceived.Items.Add(received);
            }
 
*/            
        }


        private void buttonGui_Click(object sender, EventArgs e)
        {
            listBoxReceived.Items.Clear();
            string lenh = textBoxNhapDuLieu.Text;
            lenh = lenh.Replace("\r\n", ";");
            string[] arrString = lenh.Split(';');//string[] arrString = lenh.Split(new char[] {'\n'})
            int i = arrString.Length;
            for (int j = 0; j < i; j++)
            {
                if (arrString[j] == "D" || arrString[j] == "M")
                {
                    if (arrString[j] == "D")
                    {
                        serialPort1.Write("#turnservo1open*");
                        Console.Write("#turnservo1open*");
                    }
                    if (arrString[j] == "M")
                    {
                        serialPort1.Write("#turnservo1close*");
                        Console.Write("#turnservo1close*");
                    }
                }
                if (arrString[j] != "D" && arrString[j] != "M")
                {
                    //Console.Write("\n" + arrString[j]);
                    serialPort1.WriteLine("\n" + arrString[j]);
                }
                //read data from serial port
                //save the input from the Arduino
                string received = String.Empty;
                try
                {
                    received = serialPort1.ReadExisting();//ReadLine
                }
                catch (TimeoutException)
                {
                    received = "Timeout Exception";
                }
                listBoxReceived.Items.Add(received);
                //Console.WriteLine(received);
                //Set poll frequency to 100Hz
                while (true)
                //For(int k = 0; k < 1000; k++)
                {
                    Thread.Sleep(10);
                    if (received.Contains("ABS") || received.Contains("#turnservo1open") || received.Contains("#turnservo1close")) break;
                    else
                    {
                        received = serialPort1.ReadExisting();
                        //Console.WriteLine(received);
                        //Set poll frequency to 100Hz
                    }
                }
            }
        }
        
        private void X_Up_Click(object sender, EventArgs e)
        {
            tinhieuduong = 0;
            string giatri = ImportXStep.Text;
            SetupStep[0] = Convert.ToInt16(giatri);
            CongToaDo();
        }
        
        private void X_Down_Click(object sender, EventArgs e)
        {
            tinhieuam = 0;
            string giatri = ImportXStep.Text;
            SetupStep[0] = Convert.ToInt16(giatri);
            TruToaDo();
        }

        private void Y_Up_Click(object sender, EventArgs e)
        {
            tinhieuduong = 1;
            string giatri = ImportYStep.Text;
            SetupStep[1] = Convert.ToInt16(giatri);
            CongToaDo();
        }

        private void Y_Down_Click(object sender, EventArgs e)
        {
            tinhieuam = 1;
            string giatri = ImportYStep.Text;
            SetupStep[1] = Convert.ToInt16(giatri);
            TruToaDo();
        }
        
        private void Z_Up_Click(object sender, EventArgs e)
        {
            tinhieuduong = 2;
            string giatri = ImportZStep.Text;
            SetupStep[2] = Convert.ToInt16(giatri);
            CongToaDo();
        }

        private void Z_Down_Click(object sender, EventArgs e)
        {
            tinhieuam = 2;
            string giatri = ImportZStep.Text;
            SetupStep[2] = Convert.ToInt16(giatri);
            TruToaDo();
        }
        
        private void E_Up_Click(object sender, EventArgs e)
        {
            tinhieuduong = 3;
            string giatri = ImportEStep.Text;
            SetupStep[3] = Convert.ToInt16(giatri);
            CongToaDo();
        }
        
        private void E_Down_Click(object sender, EventArgs e)
        {
            tinhieuam = 3;
            string giatri = ImportEStep.Text;
            SetupStep[3] = Convert.ToInt16(giatri);
            TruToaDo();
        }
        
        private void H_Up_Click(object sender, EventArgs e)
        {
            tinhieuduong = 4;
            string giatri = ImportHStep.Text;
            SetupStep[4] = Convert.ToInt16(giatri);
            CongToaDo();
        }

        private void H_Down_Click(object sender, EventArgs e)
        {
            tinhieuam = 4;
            string giatri = ImportHStep.Text;
            SetupStep[4] = Convert.ToInt16(giatri);
            TruToaDo();
        }

        private void CongToaDo()
        {
            td[tinhieuduong] += SetupStep[tinhieuduong];
            if (td[tinhieuduong] > 220)
            {
                td[tinhieuduong] = 220;
                MessageBox.Show("Giới hạn biên dương");
            }
            string convertstring;
            convertstring = td[tinhieuduong].ToString();
            if (tinhieuduong == 0)
            {
                serialPort1.WriteLine("\n" + "G01 X" + convertstring);
            }
            if (tinhieuduong == 1)
            {
                serialPort1.WriteLine("\n" + "G01 Y" + convertstring);
            }
            if (tinhieuduong == 2)
            {
                serialPort1.WriteLine("\n" + "G01 Z" + convertstring);
            }
            if (tinhieuduong == 3)
            {
                serialPort1.WriteLine("\n" + "G01 E" + convertstring);
            }
            if (tinhieuduong == 4)
            {
                serialPort1.WriteLine("\n" + "G01 H" + convertstring);
            }
        }

        private void TruToaDo()
        {
            td[tinhieuam] -= SetupStep[tinhieuam];
            if (td[tinhieuam] < -220)
            {
                td[tinhieuam] = -220;
                MessageBox.Show("Giới hạn biên âm ");
            }
            string convertstring;
            convertstring = td[tinhieuam].ToString();
            if (tinhieuam == 0)
            {
                serialPort1.WriteLine("\n" + "G01 X" + convertstring);
            }
            if (tinhieuam == 1)
            {
                serialPort1.WriteLine("\n" + "G01 Y" + convertstring);
            }
            if (tinhieuam == 2)
            {
                serialPort1.WriteLine("\n" + "G01 Z" + convertstring);
            }
            if (tinhieuam == 3)
            {
                serialPort1.WriteLine("\n" + "G01 E" + convertstring);
            }
            if (tinhieuam == 4)
            {
                serialPort1.WriteLine("\n" + "G01 H" + convertstring);
            }
        }

        private void ServoOff_Click(object sender, EventArgs e)
        {
            string close = "#turnservo1close*";
            //listBoxReceived.Items.Add("\n"+ "Servo Off");
            serialPort1.WriteLine(close);
        }

        private void ServoOn_Click(object sender, EventArgs e)
        {
            string open = "#turnservo1open*";
            serialPort1.WriteLine(open); 
            Console.Write("#turnservo1open*");
        }

        private void Record_Click(object sender, EventArgs e)
        {
            string CodeMustSave = textBoxNhapDuLieu.Text;
            MessageBox.Show(listBoxReceived.Text);
            using (StreamWriter Writer = new StreamWriter(@"D:\MachineVisionTool - Copy\FileWrite\" + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + ".txt", false))
            {
                Writer.WriteLine(CodeMustSave);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                string fileName;
                fileName = op.FileName;
                using (StreamReader readFile = new StreamReader(fileName))
                {
                    textBoxNhapDuLieu.Text = readFile.ReadToEnd();
                }
            }
        }

        private void Home_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("\n" + "G01 X0 Y0 Z0 E0 H0");
            for (int i = 0; i < 5; i++)
            {
                td[i] = 0;
            }
        }

        private void buttonAutomatic_Click(object sender, EventArgs e)
        {
            //Y:0.4030851/Y1, Z:0.4125/Z1, X:0.8114/X1, E:3.86/E1, H:0.9589/H1
            int i = phi.Count / 5;
            double[] arr = new double[5];
            string[] str = new string[5];
            for(int j=0;j<i;j++)
            {
                for(int h=0;h<5;h++)
                {
                    arr[h] = phi[h+j*5];
                    str[h] = Convert.ToString(arr[h]);
                    
                }
                double ar = arr[1] + 40;
                double arx = arr[2] -10;
                double arh = arr[4]-10;
                string st = Convert.ToString(ar);
                string stx = Convert.ToString(arx);
                string sth = Convert.ToString(arh);
                string[] strg = new string[11];
                //strg[0] = "G01 X" + stx+ " E" + str[3] + " H" + str[4];
                //strg[1] = "G01 Y" + str[0] + " Z" + st;
                //strg[2] = "M";
                //strg[3] = "G01 X" + str[2]+" Z"+str[1];
                //strg[4] = "D";
                //strg[5] = "G01 Z-150";
                //strg[6] = "G01 Y70";
                //strg[7] = "G01 Z" + str[1];
                //strg[8] = "M";
                //strg[9] = "G01 Z-150"+" X"+str[2];
                //strg[10] = "D";

                strg[0] = "G01 Y" + str[0]+" F20000";
                strg[1] = "G01 X" + stx + " Z" + st + " H" + str[4];
                strg[2] = "M";
                strg[3] = "G01 X" + str[2] + " Z" + str[1];
                strg[4] = "D";
                strg[5] = "G01 Z-150"+" H"+arh;
                strg[6] = "G01 Y70";
                strg[7] = "G01 Z" + str[1]+ " H" + str[4]; ;
                strg[8] = "M";
                strg[9] = "G01 Z-150" + " X" + str[2]+ " H" + arh;
                strg[10] = "D";
                for (int k = 0; k < 11; k++)
                {
                    if (strg[k] == "D" || strg[k] == "M")
                    {
                        if (strg[k] == "D")
                        {
                            serialPort1.Write("#turnservo1open*");
                        }
                        if (strg[k] == "M")
                        {
                            //Console.Write("#turnservo1close*");
                            serialPort1.Write("#turnservo1close*");
                        }
                    }
                    if (strg[k] != "D" && strg[k] != "M")
                    {
                        serialPort1.WriteLine("\n" + strg[k]);
                    }
                    string received = String.Empty;
                    try
                    {
                        received = serialPort1.ReadExisting();
                    }
                    catch (TimeoutException)
                    {
                        received = "Timeout Exception";
                    }
                    listBoxReceived.Items.Add(received);
                    while (true)
                    {
                        Thread.Sleep(10);
                        if (received.Contains("ABS") || received.Contains("#turnservo1open") || received.Contains("#turnservo1close")) break;
                        else
                        {
                            received = serialPort1.ReadExisting();
                        }
                    }
                }
            }
        }
        
        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxNhapDuLieu.Clear();
        }
        int idxSaveImage = 0;
        private void buttonCapture_Click(object sender, EventArgs e)
        {
            //luu rgb color
            string rgb_path = @"F:/DATN/Data anh/RGB/capture_" + idxSaveImage.ToString() + ".bmp";
            pictureBoxColor.Image.Save(rgb_path, System.Drawing.Imaging.ImageFormat.Bmp);
            idxSaveImage++;
        }

        private void AutoImage_Click(object sender, EventArgs e)
        {
            //openFileDialog.Filter = "File Ply(*.ply)|*.ply|All File(*.*)|*.*";
            //openFileDialog.FilterIndex = 2;
            //openFileDialog.InitialDirectory = "C:\\Users\\Chien PC\\Desktop\\Models\\Models";
            //tabControl1.SelectedIndex = 1;
            ////openFileDialog.RestoreDirectory = true;
            //if (openFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    reader.SetFileName(openFileDialog.FileName);
            //    reader.Update();
            //    scenePolyData.ShallowCopy(reader.GetOutput());
            //    vtkVertexGlyphFilter gly = vtkVertexGlyphFilter.New();
            //    gly.SetInput(scenePolyData);
            //    gly.Update();

            //    mapper.SetInputConnection(gly.GetOutputPort());
            //    actor.SetMapper(mapper);
            //    Renderer.RemoveAllViewProps();
            //    Renderer.AddActor(actor);
            //    Renderer.SetBackground(0.1804, 0.5451, 0.3412);
            //    RenderWindow.AddRenderer(Renderer);
            //    RenderWindow.SetParentId(pictureBoxPointCloud.Handle);
            //    RenderWindow.SetSize(pictureBoxPointCloud.Width, pictureBoxPointCloud.Height);
            //    RenderWindow.Render();

            //    //Iren.SetRenderWindow(RenderWindow);
            //    //Iren.SetInteractorStyle(style);
            //    //Iren.Start();
            //}
            kinectViewer = true;
            Thread.Sleep(2000);
            getPointsCloud();
            tabControl1.SelectedIndex = 1;
            if (scenePolyData.GetNumberOfPoints() > 0)
            {
                vtkVertexGlyphFilter GlyphF = vtkVertexGlyphFilter.New();
                GlyphF.SetInput(scenePolyData);
                GlyphF.Update();
                vtkPolyDataMapper mpper = vtkPolyDataMapper.New();
                mpper.SetInputConnection(GlyphF.GetOutputPort());
                vtkActor atr = vtkActor.New();
                atr.SetMapper(mpper);
                //Renderer.RemoveAllViewProps();
                Renderer.AddActor(atr);
                Renderer.ResetCamera();
                RenderWindow.Render();
            }


            // cat vung nhin thay
            vtkPolyData receiveImageTo = vtkPolyData.New();
            vtkPolyData receiveImageTo1 = vtkPolyData.New();
            receiveImageTo.ShallowCopy(scenePolyData);
            vtkPoints convertPLDtoP = vtkPoints.New();
            vtkPoints limRange = vtkPoints.New();
            convertPLDtoP = receiveImageTo.GetPoints();
            double[] a, b, c, d = new double[3];
            b = convertPLDtoP.GetPoint(0);
            c = convertPLDtoP.GetPoint(0);
            color.SetNumberOfComponents(3);
            vtkDataArray SaveColor = receiveImageTo.GetPointData().GetScalars();

            for (int i = 0; i < receiveImageTo.GetNumberOfPoints(); i++)
            {
                a = receiveImageTo.GetPoint(i);
                d = SaveColor.GetTuple3(i);                         //3 color
                if (a[0] >= b[0]) { b[0] = a[0]; }
                if (a[1] >= b[1]) { b[1] = a[1]; }
                if (a[2] >= b[2]) { b[2] = a[2]; }
                if (c[0] >= a[0]) { c[0] = a[0]; }
                if (c[1] >= a[1]) { c[1] = a[1]; }
                if (c[2] >= a[2]) { c[2] = a[2]; }
                if ((a[0] > -20 && a[0] < 33) && (a[1] > 56.3 && a[1] < 70) && (a[2] > -5 && a[2] < 30)/*(a[0] > -20 && a[0] < 30) && (a[1] > 30 && a[1] < 71) && (a[2] > -5 && a[2] < 30)*//*(a[0] > 70 && a[0] < 510) && (a[1] > 70 && a[1] < 423) && (a[2] < 900 && a[2] > 400)*/)//x ngnag, y doc, z dung
                {
                    limRange.InsertNextPoint(a[0], a[1], a[2]);
                    color.InsertNextTuple3(d[0], d[1], d[2]);
                }
            }
            vtkVertexGlyphFilter Gly1 = new vtkVertexGlyphFilter();
            vtkPolyDataMapper Mapper1 = vtkPolyDataMapper.New();
            vtkActor Actor1 = new vtkActor();
            receiveImageTo1.SetPoints(limRange);
            receiveImageTo1.GetPointData().SetScalars(color);
            scenePolyData.DeleteCells();
            scenePolyData.ShallowCopy(receiveImageTo1);
            Gly1.SetInput(receiveImageTo1);
            Gly1.Update();
            Mapper1.SetInputConnection(Gly1.GetOutputPort());
            Actor1.SetMapper(Mapper1);
            //Renderer.RemoveAllViewProps();
            Renderer.AddActor(Actor1);
            RenderWindow.Render();
            


            //phan doan
            var cloud = new PointCloudOfXYZ();
            vtkPolyData polydata = vtkPolyData.New();
            polydata.ShallowCopy(scenePolyData);
            double[] toado = new double[3];
            var pointXYZ = new PclSharp.Struct.PointXYZ();
            for (int j = 0; j <= polydata.GetNumberOfPoints(); j++)
            {
                toado = polydata.GetPoint(j);
                pointXYZ.X = (float)toado[0];
                pointXYZ.Y = (float)toado[1];
                pointXYZ.Z = (float)toado[2];
                cloud.Add(pointXYZ);
            }

            using (var clusterIndices = new VectorOfPointIndices())
            {


                using (var vg = new VoxelGridOfXYZ())
                {
                    // vg.SetInputCloud(cloud);
                    //vg.LeafSize = new Vector3(0.01f);

                    var cloudFiltered = new PointCloudOfXYZ();
                    // vg.filter(cloudFiltered);

                    cloudFiltered = cloud;
                    using (var seg = new SACSegmentationOfXYZ()
                    {
                        OptimizeCoefficients = true,
                        ModelType = SACModel.Plane,
                        MethodType = SACMethod.RANSAC,
                        MaxIterations = 1000,
                        DistanceThreshold = /*0.35*/0.75,//0.5
                    })
                    using (var cloudPlane = new PointCloudOfXYZ())
                    using (var coefficients = new PclSharp.Common.ModelCoefficients())// hệ số mẫu
                    using (var inliers = new PointIndices())// danh mục các điểm
                    {

                        int i = 0;
                        int nrPoints = cloudFiltered.Points.Count;// nrPoints được gán số lượng các điểm pointcloud

                        while (cloudFiltered.Points.Count > 0.3/*0.06*/ * nrPoints)
                        {
                            seg.SetInputCloud(cloudFiltered);
                            seg.Segment(inliers, coefficients);
                            if (inliers.Indices.Count == 0)
                                Assert.Fail("could not estimate a planar model for the given dataset");

                            using (var extract = new ExtractIndicesOfXYZ() { Negative = false })//khai báo danh mục các phần ảnh
                            {
                                extract.SetInputCloud(cloudFiltered);// thiết lập các điểm pointcloud đưa vào extract
                                extract.SetIndices(inliers.Indices);

                                extract.filter(cloudPlane);// cloudPlane là đầu ra

                                extract.Negative = true;
                                var cloudF = new PointCloudOfXYZ();
                                extract.filter(cloudF);// cloudF là các điểm đầu ra

                                cloudFiltered.Dispose();
                                cloudFiltered = cloudF;

                            }

                            i++;
                        }
                        
                        vtkPoints Point = vtkPoints.New();
                        for (int k = 0; k <= cloudFiltered.Points.Count; k++)
                        {
                            Point.InsertNextPoint(cloudFiltered.Points[k].X, cloudFiltered.Points[k].Y, cloudFiltered.Points[k].Z);

                        }

                        Renderer.RemoveAllViewProps();
                        vtkPolyData poly = vtkPolyData.New();
                        poly.SetPoints(Point);
                        vtkVertexGlyphFilter gly2 = vtkVertexGlyphFilter.New();
                        gly2.SetInput(poly);
                        gly2.Update();
                        mapper.SetInputConnection(gly2.GetOutputPort());
                        actor.SetMapper(mapper);
                        actor.GetProperty().SetColor(1, 1, 1);
                        //Renderer.RemoveAllViewProps();
                        Renderer.AddActor(actor);
                        //RenderWindow.AddRenderer(Renderer);
                        RenderWindow.Render();

                        //Assert.IsTrue(i > 1, "Didn't find more than 1 plane");
                        var tree = new PclSharp.Search.KdTreeOfXYZ();
                        tree.SetInputCloud(cloudFiltered);

                        using (var ec = new EuclideanClusterExtractionOfXYZ
                        {
                            ClusterTolerance = /*3.25*/3.5,
                            MinClusterSize = 200,
                            MaxClusterSize = 3000,//25000,
                        })
                        {
                            ec.SetSearchMethod(tree);// dùng phương pháp tree
                            ec.SetInputCloud(cloudFiltered);// ec nhận giá trị các điểm cloudFiltered
                            ec.Extract(clusterIndices);// đưa kết quả ra clusterIndices
                        }
                        //khi đã phân đoạn được các vật thể bắt đầu tách ra
                        var Cluster = new List<PointCloudOfXYZ>();
                        foreach (var pis in clusterIndices)// pis là số lượng các vật thể, mỗi vật chứa 1 cụm điểm ảnh
                        {
                            //using (var cloudCluster = new PointCloudOfXYZ())// cloudCluster là các điểm ảnh trong từng vật thể
                            var cloudCluster = new PointCloudOfXYZ();
                            {
                                foreach (var pit in pis.Indices)// xét trong từng vật thể
                                    cloudCluster.Add(cloudFiltered.Points[pit]);

                                cloudCluster.Width = cloudCluster.Points.Count;
                                cloudCluster.Height = 1;
                                //Cluster.Add(cloudCluster);
                            }
                            Cluster.Add(cloudCluster);
                        }

                        var Cluster1 = new List<PointCloudOfXYZ>();
                        foreach (var pis1 in Cluster)
                        {
                            var pointcloudXYZ = new PointCloudOfXYZ();
                            pointcloudXYZ = pis1;
                            var pointcloudXYZ1 = new PointCloudOfXYZ();
                            var sor = new StatisticalOutlierRemovalOfXYZ();
                            sor.SetInputCloud(/*cloudFiltered*/pointcloudXYZ);
                            sor.MeanK = 50;
                            sor.StdDevMulThresh = 2.7;//phai 7, giua tren 4, chinh giua 7, 1.25
                            sor.filter(pointcloudXYZ1);
                            Cluster1.Add(pointcloudXYZ1);
                            pclOfXYZ.Add(pointcloudXYZ1);
                        }


                        for (int k = 0; k < Cluster1.Count; k++)
                        {
                            vtkPoints poin = vtkPoints.New();
                            PclSharp.Std.Vector<PclSharp.Struct.PointXYZ> PointXYZ;
                            PointXYZ = Cluster1[k].Points;
                            for (int h = 0; h < PointXYZ.Count; h++)
                            {
                                poin.InsertNextPoint(PointXYZ[h].X, PointXYZ[h].Y, PointXYZ[h].Z);
                            }
                            point1.Add(poin);

                        }

                        
                        //Renderer.RemoveAllViewProps();
                        Console.WriteLine("so vat phat hien dc =" + point1.Count);
                        for (int m = 0; m < point1.Count; m++)
                        {
                            vtkPolyData Poly1 = vtkPolyData.New();
                            vtkVertexGlyphFilter Gly2 = vtkVertexGlyphFilter.New();
                            vtkPolyDataMapper Mapper2 = vtkPolyDataMapper.New();
                            vtkActor Actor2 = vtkActor.New();
                            Poly1.SetPoints(point1[m]);
                            Gly2.SetInput(Poly1);
                            Gly2.Update();
                            Mapper2.SetInputConnection(Gly2.GetOutputPort());
                            Actor2.SetMapper(Mapper2);
                            if (m == 0)
                            {
                                Actor2.GetProperty().SetColor(1.0, 0.0, 0.0);
                            }
                            if (m == 1)
                            {
                                Actor2.GetProperty().SetColor(1.0, 0.5, 0.0);
                            }
                            if (m == 2)
                            {
                                Actor2.GetProperty().SetColor(1.0, 0.5, 0.5);
                            }
                            if (m == 3)
                            {
                                Actor2.GetProperty().SetColor(0.0, 1.0, 0.0);
                            }
                            if (m == 4)
                            {
                                Actor2.GetProperty().SetColor(0.0, 1.0, 0.5);
                            }
                            if (m == 6)
                            {
                                Actor2.GetProperty().SetColor(0.5, 1.0, 0.5);
                            }
                            if (m == 7)
                            {
                                Actor2.GetProperty().SetColor(0.0, 0.0, 1.0);
                            }
                            if (m == 8)
                            {
                                Actor2.GetProperty().SetColor(0.5, 0.0, 1.0);
                            }
                            if (m == 9)
                            {
                                Actor2.GetProperty().SetColor(0.5, 0.5, 0.5);
                            }
                            if (m == 10)
                            {
                                Actor2.GetProperty().SetColor(0.1, 0.1, 0.1);
                            }
                            if (m == 11)
                            {
                                Actor2.GetProperty().SetColor(0.2, 0.2, 0.2);
                            }
                            if (m == 12)
                            {
                                Actor2.GetProperty().SetColor(0.3, 0.3, 0.3);
                            }
                            if (m == 13)
                            {
                                Actor2.GetProperty().SetColor(0.4, 0.4, 0.4);
                            }
                            if (m == 14)
                            {
                                Actor2.GetProperty().SetColor(0.6, 0.6, 0.6);
                            }
                            if (m == 15)
                            {
                                Actor2.GetProperty().SetColor(0.7, 0.7, 0.7);
                            }
                            if (m == 16)
                            {
                                Actor2.GetProperty().SetColor(0.8, 0.8, 0.8);
                            }
                            if (m == 17)
                            {
                                Actor2.GetProperty().SetColor(0.9, 0.9, 0.9);
                            }
                            if (m == 18)
                            {
                                Actor2.GetProperty().SetColor(1.0, 1.0, 0.0);
                            }
                            if (m == 19)
                            {
                                Actor2.GetProperty().SetColor(1.0, 0.0, 1.0);
                            }
                            if (m == 20)
                            {
                                Actor2.GetProperty().SetColor(0.0, 1.0, 1.0);
                            }
                            if (m == 21)
                            {
                                Actor2.GetProperty().SetColor(1.0, 0.7, 0.4);
                            }
                            if (m > 20)
                            {
                                Actor2.GetProperty().SetColor(m * 1.0 / point1.Count, 1 - m * 1.0 / point1.Count, 0.0);
                            }

                            //Actor1.GetProperty().SetColor(m * 1.0 / point1.Count, 1 - m * 1.0 / point1.Count, 0.0);
                            Renderer.AddActor(Actor2);

                            Poly.Add(Poly1);
                            Gly.Add(Gly2);
                            Mapper.Add(Mapper2);
                            Actor.Add(Actor2);

                        }
                        RenderWindow.Render();
                    }
                }
            }
            


            //tim hinh bao
            double[] addst = new double[899];
            double[] a1, b1, c1 = new double[3];
            Renderer.RemoveAllViewProps();
            foreach (var poly in Poly)
            {
                vtkPolyData PolyYToZ = vtkPolyData.New();
                vtkPoints PointYToZ = vtkPoints.New();
                vtkVertexGlyphFilter glyYToZ = vtkVertexGlyphFilter.New();
                vtkPolyDataMapper mapperYToZ = vtkPolyDataMapper.New();
                vtkActor actYToZ = vtkActor.New();
                double[] pointIn = new double[3];
                double[] pointYToZ = new double[3];
                for (int i = 0; i < poly.GetNumberOfPoints(); i++)
                {
                    pointIn = poly.GetPoint(i);
                    pointYToZ[0] = pointIn[0];
                    pointYToZ[1] = pointIn[2];
                    pointYToZ[2] = pointIn[1];
                    PointYToZ.InsertNextPoint(pointYToZ[0], pointYToZ[1], pointYToZ[2]);
                }
                PolyYToZ.SetPoints(PointYToZ);
                glyYToZ.SetInput(PolyYToZ);
                
                mapperYToZ.SetInputConnection(glyYToZ.GetOutputPort());
                actYToZ.SetMapper(mapperYToZ);
                Renderer.AddActor(actYToZ);
                RenderWindow.Render();

                vtkPoints point = vtkPoints.New();
                vtkPolyData polydt = vtkPolyData.New();
                vtkPolyData polydata1 = vtkPolyData.New();
                vtkActor act1 = vtkActor.New();
                vtkActor act2 = vtkActor.New();
                vtkPolyDataMapper vmapper1 = vtkPolyDataMapper.New();
                vtkPolyDataMapper vmapper2 = vtkPolyDataMapper.New();
                b1 = PolyYToZ.GetPoint(0)/*poly.GetPoint(0)*/;
                c1 = PolyYToZ.GetPoint(0)/*poly.GetPoint(0)*/;
                for (int i = 0; i < /*poly*/PolyYToZ.GetNumberOfPoints(); i++)
                {
                    a1 = /*poly*/PolyYToZ.GetPoint(i);
                    if (a1[0] >= b1[0]) { b1[0] = a1[0]; }  // xmax
                    if (a1[1] >= b1[1]) { b1[1] = a1[1]; }  // ymax
                    if (a1[2] >= b1[2]) { b1[2] = a1[2]; }  // zmax
                    if (c1[0] >= a1[0]) { c1[0] = a1[0]; }  // xmin
                    if (c1[1] >= a1[1]) { c1[1] = a1[1]; }  // ymin
                    if (c1[2] >= a1[2]) { c1[2] = a1[2]; }  // zmin
                }
                //Renderer.RemoveAllViewProps();
                vtkPlane plane = vtkPlane.New();
                plane.SetOrigin(0.0, /*c[1]*/0.0, /*0.0*/c1[2]);
                plane.SetNormal(0.0, /*1.0*/0.0, 1.0);
                double[] p = new double[3];
                double[] projected = new double[3];
                for (int i = 0; i < /*poly*/PolyYToZ.GetNumberOfPoints(); i++)
                {
                    p = /*poly*/PolyYToZ.GetPoint(i);

                    IntPtr pP = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * 3);
                    IntPtr pProjected = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * 3);
                    Marshal.Copy(p, 0, pP, 3);
                    Marshal.Copy(projected, 0, pProjected, 3);

                    // NOTE: normal assumed to have magnitude 1
                    plane.ProjectPoint(pP, pProjected);
                    Marshal.Copy(pProjected, projected, 0, 3);
                    Marshal.FreeHGlobal(pP);
                    Marshal.FreeHGlobal(pProjected);
                    point.InsertNextPoint(projected[0], projected[1], projected[2]);
                }
                polydt.SetPoints(point);
                vtkPoints poi = vtkPoints.New();
                vtkPoints poi1 = vtkPoints.New();
                vtkPolyData polydat = vtkPolyData.New();
                vtkPolyData polydat2 = vtkPolyData.New();
                vtkCellArray cellarray1 = vtkCellArray.New();
                vtkCellArray cellarray2 = vtkCellArray.New();
                vtkPolyLine PolyLine = vtkPolyLine.New();
                vtkPolyLine PolyLine2 = vtkPolyLine.New();
                vtkRenderer Ren = vtkRenderer.New();
                double[,] saveArea = new double[899, 6];
                double[,] box = new double[,] {
                    {0,0,0 },
                    {10,0,0 },
                    {0,0,0 },
                    {0,10,0 },
                    {0,0,0 },
                    {0,0,10 },
                };
                //Console.WriteLine("b ");
                for (int k = 0; k < 6; k++)
                {
                    poi.InsertNextPoint(box[k, 0], box[k, 1], box[k, 2]);
                }

                PolyLine.GetPointIds().SetNumberOfIds(6);
                for (int u = 0; u < 6; u++)
                {
                    PolyLine.GetPointIds().SetId(u, u);
                }
                cellarray1.InsertNextCell(PolyLine);
                polydat.SetPoints(poi);
                polydat.SetLines(cellarray1);
                vmapper1.SetInput(polydat);
                act1.SetMapper(vmapper1);
                act1.GetProperty().SetColor(1, 1, 1);
                act1.GetProperty().SetLineWidth(2);
                Renderer.AddActor(act1);
                //RenderWindow.Render();
                //thuc hien phep quay quanh truc z
                for (int i = 0; i < 899; i++)
                {
                    vtkPoints poin1 = vtkPoints.New();
                    double[] t = new double[3];
                    double[] s = new double[3];
                    double dai, rong, dt;
                    for (int j = 0; j < polydt.GetNumberOfPoints(); j++)
                    {

                        t = polydt.GetPoint(j);
                        Matrix4x4 mat = new Matrix4x4();
                        mat.M11 = (float)Math.Cos(2 * i * Math.PI / 3600);
                        mat.M12 = /*0*/-(float)Math.Sin(2 * i * Math.PI / 3600);
                        mat.M13 = 0/*(float)(Math.Sin(2 * i * Math.PI / 3600))*/;
                        mat.M14 = 0;
                        mat.M21 = /*0*/(float)Math.Sin(2 * i * Math.PI / 3600);
                        mat.M22 = /*1*/(float)Math.Cos(2 * i * Math.PI / 3600);
                        mat.M23 = 0;
                        mat.M24 = 0;
                        mat.M31 = 0/*-(float)Math.Sin(2 * i * Math.PI / 3600)*/;
                        mat.M32 = 0;
                        mat.M33 = 1/*(float)Math.Cos(2 * i * Math.PI / 3600)*/;
                        mat.M34 = 0;
                        mat.M41 = 0;
                        mat.M42 = 0;
                        mat.M43 = 0;
                        mat.M44 = 1;
                        Matrix4x4 matbd = new Matrix4x4((float)t[0], (float)t[1], (float)t[2], 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        Matrix4x4 mats = new Matrix4x4();
                        mats = matbd * mat;
                        s[0] = mats.M11;
                        s[1] = mats.M12;
                        s[2] = mats.M13;
                        poin1.InsertNextPoint(s[0], s[1], s[2]);
                    }
                    polydata1.SetPoints(poin1);

                    double[] d1, f, g = new double[3];
                    d1 = polydata1.GetPoint(0);
                    f = polydata1.GetPoint(0);
                    for (int h = 0; h < polydata1.GetNumberOfPoints(); h++)
                    {
                        g = polydata1.GetPoint(h);
                        if (g[0] >= d1[0]) { d1[0] = g[0]; }  // xmax
                        if (g[1] >= d1[1]) { d1[1] = g[1]; }  // ymax
                        if (g[2] >= d1[2]) { d1[2] = g[2]; }  // zmax
                        if (f[0] >= g[0]) { f[0] = g[0]; }  // xmin
                        if (f[1] >= g[1]) { f[1] = g[1]; }  // ymin
                        if (f[2] >= g[2]) { f[2] = g[2]; }  // zmin
                        saveArea[i, 0] = f[0];
                        saveArea[i, 1] = f[1];
                        saveArea[i, 2] = f[2];
                        saveArea[i, 3] = d1[0];
                        saveArea[i, 4] = d1[1];
                        saveArea[i, 5] = d1[2];
                    }

                    dai = Math.Sqrt((d1[0] - f[0]) * (d1[0] - f[0]));
                    rong = Math.Sqrt((d1[1] - f[1]) * (d1[1] - f[1]));
                    dt = dai * rong;
                    addst[i] = dt;

                }
                double min = addst[0];
                int index = 0;
                for (int i = 0; i < 899; i++)
                {
                    if (addst[i] <= min)
                    {
                        index = i;
                        min = addst[i];
                    }
                }
                double[,] box1 = new double[,] {
                        {saveArea[index, 0],saveArea[index, 1],saveArea[index, 2] },
                        {saveArea[index, 3],saveArea[index, 1],saveArea[index, 2] },
                        {saveArea[index, 3],saveArea[index, 4],saveArea[index, 2] },
                        {saveArea[index, 0],saveArea[index, 4],saveArea[index, 2] },
                        {saveArea[index, 0],saveArea[index, 1],saveArea[index, 2] },
                    };
                for (int k = 0; k < 5; k++)
                {
                    poi1.InsertNextPoint(box1[k, 0], box1[k, 1], box1[k, 2]);
                }
                double[] retur = new double[3];
                double[,] s1 = new double[5, 3];

                vtkPoints poi2 = vtkPoints.New();
                for (int j = 0; j < poi1.GetNumberOfPoints(); j++)
                {
                    retur = poi1.GetPoint(j);
                    Matrix4x4 mat = new Matrix4x4();
                    mat.M11 = (float)Math.Cos(-2 * index * Math.PI / 3600);
                    mat.M12 = -(float)Math.Sin(-2 * index * Math.PI / 3600);
                    mat.M13 = 0;
                    mat.M14 = 0;
                    mat.M21 = (float)Math.Sin(-2 * index * Math.PI / 3600);
                    mat.M22 = (float)Math.Cos(-2 * index * Math.PI / 3600);
                    mat.M23 = 0;
                    mat.M24 = 0;
                    mat.M31 = 0;
                    mat.M32 = 0;
                    mat.M33 = 1;
                    mat.M34 = 0;
                    mat.M41 = 0;
                    mat.M42 = 0;
                    mat.M43 = 0;
                    mat.M44 = 1;
                    Matrix4x4 matsource = new Matrix4x4((float)retur[0], (float)retur[1], (float)retur[2], 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    Matrix4x4 matdestination = new Matrix4x4();
                    matdestination = matsource * mat;
                    s1[j, 0] = matdestination.M11;
                    s1[j, 1] = matdestination.M12;
                    s1[j, 2] = matdestination.M13;
                }
                double[,] box2 = new double[,]{
                    {s1[0,0],s1[0,1],s1[0,2] },
                    {s1[1,0],s1[1,1],s1[1,2] },
                    {s1[2,0],s1[2,1],s1[2,2] },
                    {s1[3,0],s1[3,1],s1[3,2] },
                    {s1[4,0],s1[4,1],s1[4,2] },
                    {s1[4,0],s1[4,1],b1[2] },
                    {s1[1,0],s1[1,1],b1[2] },
                    {s1[1,0],s1[1,1],s1[1,2] },
                    {s1[1,0],s1[1,1],b1[2] },
                    {s1[2,0],s1[2,1],b1[2] },
                    {s1[2,0],s1[2,1],s1[2,2] },
                    {s1[2,0],s1[2,1],b1[2] },
                    {s1[3,0],s1[3,1],b1[2] },
                    {s1[3,0],s1[3,1],s1[3,2] },
                    {s1[3,0],s1[3,1],b1[2] },
                    {s1[4,0],s1[4,1],b1[2] },
                };
                for (int j = 0; j < 16; j++)
                {
                    poi2.InsertNextPoint(box2[j, 0], box2[j, 1], box2[j, 2]);
                }
                PolyLine2.GetPointIds().SetNumberOfIds(16);
                for (int u = 0; u < 16; u++)
                {
                    PolyLine2.GetPointIds().SetId(u, u);
                }
                cellarray2.InsertNextCell(PolyLine2);
                polydat2.SetPoints(poi2);
                polydat2.SetLines(cellarray2);
                vmapper2.SetInput(polydat2);
                act2.SetMapper(vmapper2);
                act2.GetProperty().SetColor(1.0, 0.5, 0.0);
                act2.GetProperty().SetLineWidth(2);
                Renderer.AddActor(act2);
                RenderWindow.Render();
                Vector3[] vector3 = new Vector3[3];
                vector3[0].X = (float)(s1[1, 0] - s1[0, 0]);
                vector3[0].Y = (float)(s1[1, 1] - s1[0, 1]);
                vector3[0].Z = (float)(s1[1, 2] - s1[0, 2]);
                vector3[1].X = (float)(s1[3, 0] - s1[0, 0]);
                vector3[1].Y = (float)(s1[3, 1] - s1[0, 1]);
                vector3[1].Z = (float)(s1[3, 2] - s1[0, 2]);
                vector3[2].X = (float)(s1[4, 0] - s1[0, 0]);
                vector3[2].Y = (float)(s1[4, 1] - s1[0, 1]);
                vector3[2].Z = (float)(b1[2] - s1[0, 2]);
                double[] corner = new double[] { s1[0, 0], s1[0, 1]/*c[1]*/, s1[0, 2] };
                Vector3 vector3X = vector3[0];
                Vector3 vector3Y = vector3[1];
                Vector3 vector3Z = vector3[2];
                Vector3 vector3s = new Vector3();
                float max, mid, min1;
                for (int dem1 = 0; dem1 < 3; dem1++)
                {
                    for (int de = dem1 + 1; de < 3; de++)
                    {
                        if (vector3[de].Length() < vector3[dem1].Length())
                        {
                            vector3s = vector3[dem1];
                            vector3[dem1] = vector3[de];
                            vector3[de] = vector3s;
                        }
                    }
                }
                max = vector3[2].Length();
                mid = vector3[1].Length();
                min1 = vector3[0].Length();
                Console.WriteLine("max= " + max + "; mid= " + mid + "; min= " + min1);
                double[] pc = new double[4];
                pc[0] = corner[0] + (vector3[0].X + vector3[1].X + vector3[2].X) / 2;
                pc[1] = corner[1] + (vector3[0].Y + vector3[1].Y + vector3[2].Y) / 2;
                pc[2] = corner[2] + (vector3[0].Z + vector3[1].Z + vector3[2].Z) / 2;
                pc[3] = 1;
                Console.WriteLine("center[" + pc[0] + "; " + pc[1] + "; " + pc[2] + "]");
                for (int add = 0; add < 4; add++)
                {
                    endpoint.Add(pc[add]);
                }
                // ma tran hieu chuan toa do diem


            }
            disposeCenter();
            tabControl2.SelectedIndex = 1;
        }

    }
}
