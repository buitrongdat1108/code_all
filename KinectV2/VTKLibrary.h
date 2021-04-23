#pragma once

using namespace System;

namespace VTKLibrary {
	public ref class Functions
	{
		// TODO: Add your methods for this class here.
	public:
		static void planeDetection(Kitware::VTK::vtkPolyData^ pts, double inlierThreshold, int maxIters, cli::array<double>^% p_normal, cli::array<double>^% p_origin);
	};
}
