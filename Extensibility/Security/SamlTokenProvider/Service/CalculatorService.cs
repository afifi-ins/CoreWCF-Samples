// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.IdentityModel.Claims;

namespace CoreWcf.Samples.SamlTokenProvider
{
    // Service class which implements the service contract interface.
    // Added code to write output to the console window
    public class CalculatorService : ICalculatorService
    {
        static void DisplayClaims()
        {
            Console.WriteLine("******");

            foreach (ClaimSet cs in OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets)
            {
                Console.WriteLine(cs);
            }

            Console.WriteLine("******");
        }

        public double Add(double n1, double n2)
        {
            DisplayClaims();
            double result = n1 + n2;
            Console.WriteLine("Received Add({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Subtract(double n1, double n2)
        {
            DisplayClaims();
            double result = n1 - n2;
            Console.WriteLine("Received Subtract({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Multiply(double n1, double n2)
        {
            DisplayClaims();
            double result = n1 * n2;
            Console.WriteLine("Received Multiply({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Divide(double n1, double n2)
        {
            DisplayClaims();
            double result = n1 / n2;
            Console.WriteLine("Received Divide({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }
    }
}
