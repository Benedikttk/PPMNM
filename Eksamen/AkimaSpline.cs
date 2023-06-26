using System;
using static System.Math;

public class AkimaSpline
{
	private double[] x;
	private double[] y;	//array for the x and y values
	private double[] b;
	private double[] c;
	private double[] d;	//Akima coefficients
	private int n;	
	
   	private AkimaSpline(double[] x, double[] y, double[] b, double[] c, double[] d, int n)
    	{
        	this.x = x;
        	this.y = y;
        	this.b = b;
        	this.c = c;
        	this.d = d;
       	 	this.n = n;
    	}

	public static AkimaSpline Create(double[] x_data, double[] y_data) 	
	{/*Implentematation of Akima(sub-)spline interpolation for a points with x,y value*/
		int n = x_data.Length;
	
		double[] h = new double[n-1];	//Differences between the x values
		double[] p = new double[n-1];	//Slopes (dy/dx)
	
		for(int i=0;i<n-1;i++)
		{
			h[i] = x_data[i+1]-x_data[i]; //see under eq.32 in Book.
			if(h[i]<0)
				throw new ArgumentException("x values must be in increasing manner");
		}//for

		for(int i=0;i<n-1;i++)
		{/*Calculating the slope p*/	
			p[i] = (y_data[i+1]-y_data[i])/h[i];
		}//for
		
		var s = new AkimaSpline(x_data, y_data, new double[n], new double[n - 1], new double[n - 1],n); //dosent want to work if Creat is static	
		
		Array.Copy(x_data, s.x, n);
		Array.Copy(y_data, s.y, n);

		/*Now the Akami coefficients will be calculated*/
		s.b[0]=p[0];
		s.b[1]=(p[0]+p[1])/2;
		s.b[n-1]=p[n-2];
		s.b[n-2]=(p[n-2]+p[n-3])/2;

		for(int i=2; i<n-2;i++)
		{/*see eq.35*/
			double w1=Abs(p[i+1]-p[i]);
			double w2=Abs(p[i-1]-p[i-2]);
			
			if(w1+w2==0)
				s.b[i]=(p[i-1]+p[i])/2;
			else
				s.b[i]=(w1*p[i-1]+ w2*p[i])/(w1+w2);
		}//for
		/*now we calculate the c and d akima coefficients*/
		for(int i=0;i<n-1;i++)
		{		
			s.c[i] = (3*p[i]-2*s.b[i]-s.b[i+1])/h[i];
           		s.d[i] = (s.b[i+1]+s.b[i]-2*p[i])/(h[i]*h[i]);
		}//for
		return s;
	}//Create

public double Evaluate(double z)
{
    if (z < x[0])
    {
        // Handle case where z is less than the minimum x value
        return y[0];
    }
    else if (z > x[n - 1])
    {
        // Handle case where z is greater than the maximum x value
        return y[n - 1];
    }

    int i = 0;
    int j = n - 1;

    while (j - i > 1)
    {
        int m = (i + j) / 2;
        if (z > x[m])
        {
            i = m;
        }
        else
        {
            j = m;
        }
    }

    double h = z - x[i];
    double interpolatedValue = y[i] + h * (b[i] + h * (c[i] + h * d[i]));

    return interpolatedValue;
}




}//AkimaSpline
