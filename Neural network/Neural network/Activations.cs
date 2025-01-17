﻿using static NeuralNetwork.Utils;

namespace NeuralNetwork
{
	public readonly struct Activations
	{
        public readonly struct SELU 
        {
            public static double Activation(double x)
            {
                if (x<=0d)
                {
                    return 1.758d * (Math.Exp(x) - 1);
                }
                return 1.051d * x;
            }

            public static double Derivative(double x)
            {
                if (x<=0d)
                {
                    return 1.758d * Math.Exp(x);
                }

                return 1.051d;
            }
        }


        public readonly struct RELU
        {
            public static double Activation(double x)
            {
                if (x >= 0d)
                {
                    return x;
                }
                return 0d;
            }
            public static double Derivative(double x)
            {
                if (x >= 0d)
                {
                    return 1d;
                }
                return 0d;
            }
        }

        public readonly struct LeakyRELU
        {
            private const double alpha = 1d / 128d;
            public static double Activation(double x)
            {
                if (x >= 0d)
                {
                    return x;
                }
                return alpha * x;
            }
            public static double Derivative(double x)
            {
                if (x >= 0d)
                {
                    return 1d;
                }
                return alpha;
            }
        }

        public readonly struct Sigmoid
        {

            public static double Activation(double x)
            {
                return (1.0d) / (1.0d + Math.Exp(-x));
            }

            public static double Derivative(double x)
            {
                double SigX = Activation(x);
                return SigX * (1.0d - SigX);
            }
        }

        public readonly struct SILU
        {
            public static double Activation(double x)
            {
                return x * Sigmoid.Activation(x);
            }

            public static double Derivative(double x)
            {
                double SigX = Sigmoid.Activation(x);
                return SigX*(1 + x * (1-SigX));
            }
        }

        public readonly struct SoftPlus
        {
            public static double Activation(double x)
            {
                return Math.Log(1 + Math.Exp(x));
            }

            public static double Derivative(double x)
            {
                return Sigmoid.Activation(x);
            }
        }


        public readonly struct SoftMax
        {
            public static double Activation(double[] netInputs, double value)
            {
                // to make the Exp calculation safe (prevent overflow), we'll substract
                // the max value from each item ("Safe SoftMax")
                var maxVal = netInputs[0];
                for (int i = 1; i < netInputs.Length; i++)
                {
                    if (netInputs[i] > maxVal)
                        maxVal = netInputs[i];
                }
                double expsum = 0d;
                for (int i = 0; i < netInputs.Length; i++)
                {
                    expsum += Math.Exp(netInputs[i] - maxVal);
                }
                return Math.Exp(value - maxVal) / expsum;
            }

            public static double Derivative(double[] netInputs, double value)
            {
                double softmax = Activation(netInputs, value);
                return softmax * (1 - softmax);
            }
        }


        public readonly struct Tanh
        {
            public static double Activation(double x)
            {
                return Math.Tanh(x);
            }

            public static double Derivative(double x)
            {
                double val = Math.Tanh(x);
                return 1 - val * val;
            }
        }


    }
}

