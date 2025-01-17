﻿namespace NeuralNetwork
{
    [Serializable]
    public class Node
    {
        public List<Connection> inputConnections = new();
        public List<Connection> outputConnections = new();
        public double value;
        public double bias;
        public double biasDerivative = 0d;
        public double biasVelocity = 0d;
        public double gradient = 0d;
        public double errorDer = 0d;

        public Node(double value = 0d, double bias = 0d)
        {
            this.value = value;
            this.bias = bias;
        }

        /*
            UPDATE NODE DATA.
        */
        public void UpdateBias(double trainingStep, double momentum)
        {
            double velocity = biasVelocity * momentum - biasDerivative * trainingStep;
            biasVelocity = velocity;
            bias += velocity;
            biasDerivative = 0d;
        }

        public void AddBiasDerivative()
        {
            biasDerivative += gradient;
        }

        public void UpdateGradient() //this is for a hidden layer
        {
            gradient = 0d;
            for (int i = 0; i < outputConnections.Count; i++)
            {
                var con = outputConnections[i];
                gradient += con.nodeOut.gradient * con.weight;
            }
            gradient *= Activations.LeakyRELU.Derivative(GetNetActivationInput());
            Utils.ThrowWhenBadValue(gradient);
        }

        public void UpdateGradient(Layer curLayer, double target) // this is for the output layer
        {
            //speed optimization:
            //it's not necessary to calculate the formula when using
            //the cross entropy cost function with softmax activation
            //as it simplifies to "value - target"
            if (false)
            {
#pragma warning disable CS0162 // Unreachable code detected
                double[] netInputs = curLayer.GetLayerNetInputs();
                var costFunctioDerivative = Cost.CROSS_ENTROPY.CostFunctionIterationDerivative(target, value);
                var activationFunctionDerivative = Activations.SoftMax.Derivative(netInputs, GetNetActivationInput());
                gradient = costFunctioDerivative * activationFunctionDerivative;
#pragma warning restore CS0162 // Unreachable code detected
            }
            else
            {
                gradient = value - target;
            }
            Utils.ThrowWhenBadValue(gradient);
        }

        /*
            GET NODE DATA.
        */
        public double GetNetActivationInput()
        {
            double net = 0.0;
            for (int i = 0; i < inputConnections.Count; i++)
            {
                var con = inputConnections[i];
                net += con.weight * con.nodeIn.value;
            }
            Utils.ThrowWhenBadValue(net);
            return net + bias;
        }
    }
}

