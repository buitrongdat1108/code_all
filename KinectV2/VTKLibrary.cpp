#include "stdafx.h"
#include "vtkPolyData.h"
#include "vtkPoints.h"
#include "vtkSmartPointer.h"
#include "vtkVertexGlyphFilter.h"
#include "vtkRANSACPlane.h"
#include "VTKLibrary.h"

void VTKLibrary::Functions::planeDetection(Kitware::VTK::vtkPolyData^ pts, double inlierThreshold, int maxIters, cli::array<double>^% p_normal, cli::array<double>^% p_origin)
{
	vtkSmartPointer<vtkPolyData> inputPoints = vtkSmartPointer<vtkPolyData>::New();

	vtkSmartPointer<vtkPoints> points = vtkSmartPointer<vtkPoints>::New();
	cli::array<double>^ pt = gcnew cli::array<double>(3);
	for (int i = 0; i < pts->GetNumberOfPoints(); i++)
	{
		pt = pts->GetPoint(i);
		points->InsertNextPoint(pt[0], pt[1], pt[2]);
	}

	inputPoints->SetPoints(points);

	vtkSmartPointer<vtkVertexGlyphFilter> filters = vtkSmartPointer<vtkVertexGlyphFilter>::New();
	filters->SetInput(inputPoints);
	filters->Update();

	inputPoints->ShallowCopy(filters->GetOutput());

	vtkSmartPointer<vtkRANSACPlane> rANSACPlane = vtkSmartPointer<vtkRANSACPlane>::New();
	rANSACPlane->SetInlierThreshold(inlierThreshold);
	rANSACPlane->SetMaxIterations(maxIters);
	rANSACPlane->SetInputConnection(inputPoints->GetProducerPort());
	rANSACPlane->Update();

	//vtkSmartPointer<vtkPolyData> plane = vtkSmartPointer<vtkPolyData>::New();
	//plane->ShallowCopy(rANSACPlane->GetOutput());

	//get the normal and origin of the plane
	double normal[3], origin[3];
	rANSACPlane->GetNormal(normal);
	rANSACPlane->GetOrigin(origin);

	for (int i = 0; i < 3; i++)
	{
		p_normal[i] = normal[i];
		p_origin[i] = origin[i];
	}
}


