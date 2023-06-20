// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.Security
{
    // Service class which implements a duplex service contract.
    // Use an InstanceContextMode of PrivateSession to store the result
    // An instance of the service will be bound to each duplex session
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class CalculatorService : ICalculatorService
    {
        private double _result;
        private string _equation;
        private readonly ICalculatorDuplexCallback _callback = null;

        public CalculatorService()
        {
            _result = 0.0D;
            _equation = _result.ToString();
            _callback = OperationContext.Current.GetCallbackChannel<ICalculatorDuplexCallback>();
        }

        public void Clear()
        {
            _callback.Equation(_equation + " = " + _result.ToString());
            _result = 0.0D;
            _equation = _result.ToString();
        }

        public void AddTo(double n)
        {
            _result += n;
            _equation += " + " + n.ToString();
            _callback.Result(_result);
        }

        public void SubtractFrom(double n)
        {
            _result -= n;
            _equation += " - " + n.ToString();
            _callback.Result(_result);
        }

        public void MultiplyBy(double n)
        {
            _result *= n;
            _equation += " * " + n.ToString();
            _callback.Result(_result);
        }

        public void DivideBy(double n)
        {
            _result /= n;
            _equation += " / " + n.ToString();
            _callback.Result(_result);
        }
    }
}
