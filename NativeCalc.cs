using System;
using System.Runtime.InteropServices;

class NativeCalc
{
    [DllImport("calc", CallingConvention = CallingConvention.Cdecl)]
    public static extern double CalculateTotal(double price, int quantity);

    public static double GetTotal(double price, int quantity)
    {
        try
        {
            return CalculateTotal(price, quantity);
        }
        catch
        {
            return price * quantity;
        }
    }
}
