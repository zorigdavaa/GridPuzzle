using System.Collections.Generic;
using UnityEngine;
namespace ZPackage
{
    public class M
    {

        ///<summary>PI тоо 3.1415926536</summary>
        public const float PI = 3.14159265359F;

        ///<summary>e тоо 2.7182818284</summary>
        public const float e = 2.71828182846F;

        ///<summary>epsilon маш бага бутархай тоо 1.401298E-45</summary>
        public const float Epsilon = 1.401298E-45F;

        ///<summary>хамгийн их утга 3.40282347E+38</summary>
        public const float MaxVal = 3.40282347E+38F;

        ///<summary>хамгийн бага утга -3.40282347E+38</summary>
        public const float MinVal = -3.40282347E+38F;

        ///<summary>тоо биш</summary>
        public const float NaN = 0F / 0F;

        ///<summary>хасах хязгааргүй</summary>
        public const float NegInf = -1F / 0F;

        ///<summary>нэмэх хязгааргүй</summary>
        public const float PosInf = 1F / 0F;

        ///<summary>алтан харьцаа 1.61803398875</summary>
        public const float GoldenRatio = 1.61803398875F;

        ///<summary>√2 1.41421356237</summary>
        public const float Sqrt2 = 1.41421356237F;

        ///<summary>√3 1.73205080757</summary>
        public const float Sqrt3 = 1.73205080757F;

        ///<summary>√5 2.2360679775</summary>
        public const float Sqrt5 = 2.2360679775F;

        ///<summary>√6 2.44948974278</summary>
        public const float Sqrt6 = 2.44948974278F;

        ///<summary>√7 2.64575131106</summary>
        public const float Sqrt7 = 2.64575131106F;

        ///<summary>√8 2.82842712475</summary>
        public const float Sqrt8 = 2.82842712475F;

        ///<summary>degree => radian</summary>
        public const float DegRad = 0.0174532924F;

        ///<summary>radian => degree</summary>
        public const float RadDeg = 57.29578F;

        ///<summary>kilometer => mile</summary>
        public const float KmMi = 0.621371192f;

        ///<summary>mile => kilometer</summary>
        public const float MiKm = 1.609344f;

        ///<summary>meter => foot</summary>
        public const float MFt = 3.2808399f;

        ///<summary>foot => meter</summary>
        public const float FtM = 0.3048f;

        ///<summary>centimeter => inch</summary>
        public const float CmIn = 0.393700787f;

        ///<summary>inch => centimeter</summary>
        public const float InCm = 2.54f;

        ///<summary>kilogram => pound</summary>
        public const float KgLbs = 2.20462262f;

        ///<summary>pound => kilogram</summary>
        public const float LbsKg = 0.45359237f;

        ///<summary>litre => oz</summary>
        public const float LOz = 33.8140227f;

        ///<summary>oz => litre</summary>
        public const float OzL = 0.0295735296f;

        ///<summary>celsius => kelvin</summary>
        public static float Ck(float f)
        {
            return f + 273.15f;
        }

        ///<summary>celsius => fahrenheit</summary>
        public static float Cf(float f)
        {
            return f * 9f / 5f + 32f;
        }

        ///<summary>kelvin => celsius</summary>
        public static float Kc(float f)
        {
            return f - 273.15f;
        }

        ///<summary>kelvin => fahrenheit</summary>
        public static float Kf(float f)
        {
            return Cf(Kc(f));
        }

        ///<summary>fahrenheit => celsius</summary>
        public static float Fc(float f)
        {
            return (f - 32f) * 5f / 9f;
        }

        ///<summary>fahrenheit => kelvin</summary>
        public static float Fk(float f)
        {
            return Ck(Fc(f));
        }

        ///<summary>0-тэй тэнцүүг d-р шалгана</summary>
        public static bool IsZero(float f, float d)
        {
            return Apx(f, 0f, d);
        }

        ///<summary>n-р фибоначийн тоо</summary>
        public static int Fibonacci(int n)
        {
            int res = n, n1 = 0, n2 = 1;
            if (n > 1)
                for (int i = 2; i <= n; i++)
                {
                    res = n1 + n2;
                    n2 = n1;
                    n1 = res;
                }
            return res;
        }

        ///<summary>a, b-н ХИ ерөнхий хуваагч</summary>
        public static int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
                if (a > b)
                    a %= b;
                else
                    b %= a;
            return a == 0 ? b : a;
        }

        ///<summary>a, b-н ХБ ерөнхий хуваагдагч</summary>
        public static int LCM(int a, int b)
        {
            return a * b / GCD(a, b);
        }

        ///<summary>n! n-н факториал эсвэл сэлгэмэл Pn = n!</summary>
        public static int Factorial(int n)
        {
            int res = 1;
            if (n > 1)
                for (int i = 2; i <= n; i++)
                    res *= i;
            return res;
        }

        ///<summary>гүйлгэмэл A(n,k) = n!/(n-k)!</summary>
        public static float Permutation(int n, int k)
        {
            return (float)Factorial(n) / (float)Factorial(n - k);
        }

        ///<summary>хэсэглэл C(n,k) = n!/(k!(n-k)!)</summary>
        public static float Combination(int n, int k)
        {
            return (float)Factorial(n) / (float)(Factorial(k) * Factorial(n - k));
        }

        ///<summary>квадрат тэгшитгэл ax²+bx+c = 0</summary>
        public static Vector2 QuadraticEquation(float a, float b, float c)
        {
            if (IsZero(a, 0.001f))
                return new Vector2(IsZero(b, 0.001f) ? NaN : -c / b, NaN);
            float d = b * b - 4f * a * c;
            if (d >= 0f)
                return new Vector2((-b + Mathf.Sqrt(d)) / (2f * a), (-b - Mathf.Sqrt(d)) / (2f * a));
            return new Vector2(NaN, NaN);
        }

        ///<summary>a, b-н хооронд f байна уу шалгана</summary>
        public static bool IsBet(float f, float a, float b)
        {
            return Min(a, b) <= f && f <= Max(a, b);
        }

        ///<summary>f-н эерэг утга</summary>
        public static float Abs(float f)
        {
            return Mathf.Abs(f);
        }

        ///<summary>2D Перлин шуугиан үүсгэнэ</summary>
        public static float PerlinNoise(float x, float y)
        {
            return Mathf.PerlinNoise(x, y);
        }

        ///<summary>a, b-н зөрүү</summary>
        public static float Dis(float a, float b)
        {
            return Abs(a - b);
        }

        ///<summary>a, b-н f-тэй ойрхон</summary>
        public static float Near(float f, float a, float b)
        {
            return Dis(f, a) < Dis(f, b) ? a : b;
        }

        ///<summary>a, b-н хоорондох f-г a2, b2 хооронд болгож өөрчилнө</summary>
        public static float Remap(float f, float a, float b, float a2, float b2)
        {
            return (f - a) / (b - a) * (b2 - a2) + a2;
        }

        ///<summary>a-г b-рүү d-р ойртуулна</summary>
        public static float Move(float a, float b, float d)
        {
            return Mathf.MoveTowards(a, b, d);
        }

        // PRIME NUMBER

        ///<summary>n-р анхны тоо</summary>
        public static int Prime(int n)
        {
            int res = 2;
            for (int i = 0; i <= n; res++)
                if (IsPrime(res)) i++;
            return res;
        }

        ///<summary>анхны тоо мөнүү шалгана</summary>
        public static bool IsPrime(int n)
        {
            for (int i = 2, m = n / 2; i <= m; i++)
                if (n % i == 0)
                    return false;
            return n >= 2;
        }

        // APPROXIMATELY

        ///<summary>a, b-г ойролцоог d-р шалгана</summary>
        public static bool Apx(float a, float b, float d)
        {
            return Dis(a, b) <= d;
        }

        ///<summary>a, b-г ойролцоог шалгана</summary>
        public static bool Apx(float a, float b)
        {
            return Mathf.Approximately(a, b);
        }

        // POWER

        ///<summary>bⁿ b-н n зэрэг</summary>
        public static float Pow(float b, float n)
        {
            return Mathf.Pow(b, n);
        }

        ///<summary>10ⁿ 10-н n зэрэг</summary>
        public static float Pow10(float n)
        {
            return Pow(10, n);
        }

        ///<summary>2ⁿ 2-н n зэрэг</summary>
        public static float Pow2(float n)
        {
            return Pow(2, n);
        }

        ///<summary>-1ⁿ -1-н n зэрэг</summary>
        public static float PowNeg1(float n)
        {
            return Pow(-1, n);
        }

        ///<summary>v-г 2-н n зэрэг мөнүү шалгана</summary>
        public static bool IsPow2(float v)
        {
            return Mathf.IsPowerOfTwo((int)v);
        }

        ///<summary>v-тэй ойрхон бага 2-н n зэрэг</summary>
        public static int CloPow2(float v)
        {
            return Mathf.ClosestPowerOfTwo((int)v);
        }

        ///<summary>v-тэй ойрхон их 2-н n зэрэг</summary>
        public static int NxtPow2(float v)
        {
            return Mathf.NextPowerOfTwo((int)v);
        }

        ///<summary>eⁿ e-н n зэрэг</summary>
        public static float Exp(float n)
        {
            return Mathf.Exp(n);
        }

        ///<summary>√x язгуур доор x</summary>

        public static float Sqrt(float x)
        {
            return Mathf.Sqrt(x);
        }

        ///<summary>³√x 3 язгуур доор x</summary>
        public static float Cbrt(float x)
        {
            return NthRoot(x, 3);
        }

        ///<summary>ⁿ√x n язгуур доор x</summary>
        public static float NthRoot(float x, int n)
        {
            return n % 2 == 1 && x < 0 ? -Mathf.Pow(-x, 1f / n) : Mathf.Pow(x, 1f / n);
        }

        // REPEAT

        ///<summary>f-г a, b-н хооронд давтана</summary>
        public static float Rep(float f, float a, float b)
        {
            return Rep(f - a, b - a) + a;
        }

        ///<summary>f-г 0, n-н хооронд давтана</summary>
        public static float Rep(float f, float n)
        {
            return n >= 0 ? Mathf.Repeat(f, n) : -Mathf.Repeat(-f, -n);
        }

        ///<summary>idx-г давтана</summary>
        public static int RepIdx(int idx, int n)
        {
            return (int)Rep(idx, n);
        }

        ///<summary>f-г a, b-н хооронд ойлгож давтана</summary>
        public static float PingPong(float f, float a, float b)
        {
            return PingPong(f - a, b - a) + a;
        }

        ///<summary>f-г 0, n-н хооронд ойлгож давтана</summary>
        public static float PingPong(float f, float n)
        {
            return n >= 0 ? Mathf.PingPong(f, n) : -Mathf.PingPong(-f, -n);
        }

        // ROUND

        ///<summary>x-г тоймлоно</summary>
        public static float Round(float x)
        {
            return Mathf.Round(x);
        }

        ///<summary>x-г доошоо тоймлоно</summary>
        public static float Floor(float x)
        {
            return Mathf.Floor(x);
        }

        ///<summary>x-г дээшээ тоймлоно</summary>
        public static float Ceil(float x)
        {
            return Mathf.Ceil(x);
        }

        ///<summary>x-г 0-рүү тоймлоно</summary>
        public static float Truncate(float x)
        {
            return x >= 0 ? Floor(x) : Ceil(x);
        }

        ///<summary>x-г 0-с хол тоймлоно</summary>
        public static float Sgn(float x)
        {
            return x >= 0 ? Ceil(x) : Floor(x);
        }

        ///<summary>x-г тоймлоно</summary>
        public static int RoundInt(float f)
        {
            return Mathf.RoundToInt(f);
        }

        ///<summary>x-г доошоо тоймлоно</summary>
        public static int FloorInt(float f)
        {
            return Mathf.FloorToInt(f);
        }

        ///<summary>x-г дээшээ тоймлоно</summary>
        public static int CeilInt(float f)
        {
            return Mathf.CeilToInt(f);
        }

        ///<summary>x-г 0-рүү тоймлоно</summary>
        public static int TruncateInt(float x)
        {
            return x >= 0 ? FloorInt(x) : CeilInt(x);
        }

        ///<summary>x-г 0-с хол тоймлоно</summary>
        public static int SgnInt(float x)
        {
            return x >= 0 ? CeilInt(x) : FloorInt(x);
        }

        ///<summary>x-г b-сууриар c-р эхлүүлж тоймлоно</summary>
        public static float BaseRound(float x, float b, float c = 0)
        {
            return Round((x - c) / b) * b + c;
        }

        ///<summary>x-г b-сууриар c-р эхлүүлж доошоо тоймлоно</summary>
        public static float BaseFloor(float x, float b, float c = 0)
        {
            return Floor((x - c) / b) * b + c;
        }

        ///<summary>x-г b-сууриар c-р эхлүүлж дээшээ тоймлоно</summary>
        public static float BaseCeil(float x, float b, float c = 0)
        {
            return Ceil((x - c) / b) * b + c;
        }

        // LOGARITHM

        ///<summary>logᵦ(x) логарифм b суурьтай x</summary>
        public static float Log(float x, float b)
        {
            return Mathf.Log(x, b);
        }

        ///<summary>lb(x) = log₂(x) логарифм 2 суурьтай x</summary>
        public static float Lb(float x)
        {
            return Log(x, 2);
        }

        ///<summary>ln(x) = logₑ(x) логарифм e суурьтай x</summary>
        public static float Ln(float x)
        {
            return Mathf.Log(x);
        }

        ///<summary>lg(x) = log₁₀(x) логарифм 10 суурьтай x</summary>
        public static float Lg(float x)
        {
            return Mathf.Log10(x);
        }

        // MIN

        ///<summary>a, b-н бага</summary>
        public static int Min(int a, int b)
        {
            return Mathf.Min(a, b);
        }

        ///<summary>a, b-н бага</summary>
        public static float Min(float a, float b)
        {
            return Mathf.Min(a, b);
        }

        ///<summary>vals-н бага</summary>
        public static int Min(params int[] vals)
        {
            return Mathf.Min(vals);
        }

        ///<summary>vals-н бага</summary>
        public static float Min(params float[] vals)
        {
            return Mathf.Min(vals);
        }

        // MAX

        ///<summary>a, b-н их</summary>
        public static int Max(int a, int b)
        {
            return Mathf.Max(a, b);
        }

        ///<summary>a, b-н их</summary>
        public static float Max(float a, float b)
        {
            return Mathf.Max(a, b);
        }

        ///<summary>arr-н их</summary>
        public static int Max(params int[] arr)
        {
            return Mathf.Max(arr);
        }

        ///<summary>arr-н их</summary>
        public static float Max(params float[] arr)
        {
            return Mathf.Max(arr);
        }

        // CLAMP

        ///<summary>f∈[a, b]</summary>
        public static int Clamp(int f, int a, int b)
        {
            return Mathf.Clamp(f, a, b);
        }

        ///<summary>f∈[a, b]</summary>
        public static float Clamp(float f, float a, float b)
        {
            return Mathf.Clamp(f, a, b);
        }

        ///<summary>f∈[-1, 1]</summary>
        public static float Clamp(float f)
        {
            return Mathf.Clamp(f, -1f, 1f);
        }

        ///<summary>f∈[0, n]</summary>
        public static float ClampN(float f, float n)
        {
            return Mathf.Clamp(f, 0f, n);
        }

        ///<summary>f∈[0, 1]</summary>
        public static float Clamp01(float f)
        {
            return Mathf.Clamp01(f);
        }

        ///<summary>idx∈[0, n[</summary>
        public static int ClampIdx(int idx, int n)
        {
            return Mathf.Clamp(idx, 0, n - 1);
        }

        // LERP

        ///<summary>a-г b-рүү t хувиар ойртуулна [a, b]</summary>
        public static float Lerp(float from, float to, float value)
        {
            if (value < 0.0f)
                return from;
            if (value > 1.0f)
                return to;
            return (to - from) * value + from;
        }

        ///<summary>a-г b-рүү t хувиар ойртуулна</summary>
        public static float UncLerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
        public static float LerpUnclamped(float from, float to, float value)
        {
            return (1.0f - value) * from + value * to;
        }

        ///<summary>a, b-н хоорондох t-н хувь [a, b]</summary>
        public static float InverseLerp(float from, float to, float value)
        {
            if (from < to)
            {
                if (value < from)
                    return 0.0f;
                if (value > to)
                    return 1.0f;
            }
            else
            {
                if (value < to)
                    return 1.0f;
                if (value > from)
                    return 0.0f;
            }
            return (value - from) / (to - from);
        }

        ///<summary>a, b-н хоорондох t-н хувь</summary>
        public static float InverseLerpUnclamped(float from, float to, float value)
        {
            return (value - from) / (to - from);
        }

        ///<summary>a-г b-рүү t хувиар ойртуулна _/‾[a, b]</summary>
        public static float SmoothStep(float from, float to, float value)
        {
            if (value < 0.0f)
                return from;
            if (value > 1.0f)
                return to;
            value = value * value * (3.0f - 2.0f * value);
            return (1.0f - value) * from + value * to;
        }

        ///<summary>a-г b-рүү t хувиар ойртуулна _/‾</summary>
        public static float UncSmoothStep(float a, float b, float t)
        {
            t = t * t * (3.0f - 2.0f * t);
            return a + (b - a) * t;
        }

        ///<summary>InvLerp => Lerp [a, b]</summary>
        public static float SuperLerp(float from, float to, float from2, float to2, float value)
        {
            if (from2 < to2)
            {
                if (value < from2)
                    value = from2;
                else if (value > to2)
                    value = to2;
            }
            else
            {
                if (value < to2)
                    value = to2;
                else if (value > from2)
                    value = from2;
            }
            return (to - from) * ((value - from2) / (to2 - from2)) + from;
        }

        ///<summary>InvLerp => Lerp</summary>
        public static float UncSuperLerp(float a, float b, float a2, float b2, float t)
        {
            return a + (b - a) * ((t - a2) / (b2 - a2));
        }

        ///<summary>De Casteljau-н алгоритм [зэргийн lerp]</summary>
        public static float DegLerp(float t, params float[] arr)
        {
            if (arr == null || arr.Length == 0)
                return 0;
            List<float> lis = new List<float>(arr);
            while (lis.Count > 1)
            {
                List<float> tmp = new List<float>();
                for (int i = 1; i < lis.Count; i++)
                    tmp.Add(Lerp(lis[i - 1], lis[i], t));
                lis = tmp;
            }
            return lis[0];
        }

        ///<summary>Sinerp /‾</summary>
        public static float Sinerp(float a, float b, float t)
        {
            return a + (b - a) * Mathf.Sin(t * PI * 0.5f);
        }

        ///<summary>SinLerp /‾\</summary>
        public static float SinLerp(float a, float b, float t)
        {
            return a + (b - a) * Mathf.Sin(t * PI);
        }

        ///<summary>Coserp _/</summary>
        public static float Coserp(float a, float b, float t)
        {
            return a + (b - a) * 1.0f - Mathf.Cos(t * PI * 0.5f);
        }

        ///<summary>Berp /~</summary>
        public static float Berp(float a, float b, float t)
        {
            t = Mathf.Clamp01(t);
            t = (Mathf.Sin(t * Mathf.PI * (0.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t) * (1f + (1.2f * (1f - t)));
            return a + (b - a) * t;
        }

        ///<summary>Bounce /\\^ˆ</summary>
        public static float Bounce(float a, float b, float t)
        {
            return a + (b - a) * Mathf.Abs(Mathf.Sin(PI * 2f * (t + 1f) * (t + 1f)) * (1f - t));
        }
        public static Vector3 QuadraticCurve(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 p0 = Vector3.Lerp(a, b, t);
            Vector3 p1 = Vector3.Lerp(b, c, t);
            return Vector3.Lerp(p0, p1, t);
        }
        public static Vector3 CubicCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            Vector3 p0 = QuadraticCurve(a, b, c, t);
            Vector3 p1 = QuadraticCurve(b, c, d, t);
            return Vector3.Lerp(p0, p1, t);
        }





        //                   B
        //                 /|
        //               /  |
        // hypotenuse c/    |a opposite
        //           /      |
        //         /________|
        //       A     b     C
        //          adjacent

        // TRIGONOMETRY DEGREE

        ///<summary>sine sinA = opposite/hypotenuse = a/c</summary>
        public static float Sin(float f)
        {
            return Mathf.Sin(f * DegRad);
        }

        ///<summary>cosine cosA = adjacent/hypotenuse = b/c</summary>
        public static float Cos(float f)
        {
            return Mathf.Cos(f * DegRad);
        }

        ///<summary>tangent tanA = opposite/adjacent = a/b = (a/c)/(b/c) = sinA/cosA</summary>
        public static float Tan(float f)
        {
            return Mathf.Tan(f * DegRad);
        }

        ///<summary>cosecant cscA = 1/sinA = hypotenuse/opposite = c/a</summary>
        public static float Csc(float f)
        {
            return 1f / Mathf.Sin(f * DegRad);
        }

        ///<summary>secant secA = 1/cosA = hypotenuse/adjacent = c/b</summary>
        public static float Sec(float f)
        {
            return 1f / Mathf.Cos(f * DegRad);
        }

        ///<summary>cotangent cotA = 1/tanA = adjacent/opposite = cosA/sinA = b/a</summary>
        public static float Cot(float f)
        {
            return 1f / Mathf.Tan(f * DegRad);
        }

        ///<summary>arcsine y = arcsin(x), x = sin(y)</summary>
        public static float Asin(float f)
        {
            return Mathf.Asin(f) * RadDeg;
        }

        ///<summary>arccosine y = arccos(x), x = cos(y)</summary>
        public static float Acos(float f)
        {
            return Mathf.Acos(f) * RadDeg;
        }

        ///<summary>arctangent y = arctan(x), x = tan(y)</summary>
        public static float Atan(float f)
        {
            return Mathf.Atan(f) * RadDeg;
        }

        ///<summary>arccosecant y = arccsc(x), x = csc(y)</summary>
        public static float Acsc(float f)
        {
            return Mathf.Asin(1f / f) * RadDeg;
        }

        ///<summary>arcsecant y = arcsec(x), x = sec(y)</summary>
        public static float Asec(float f)
        {
            return Mathf.Acos(1f / f) * RadDeg;
        }

        ///<summary>arccotangent y = arccot(x), x = cot(y)</summary>
        public static float Acot(float f)
        {
            return Mathf.Atan(1f / f) * RadDeg;
        }

        ///<summary>tan-н өнцөг</summary>
        public static float Atan2(float y, float x)
        {
            return Mathf.Atan2(y, x) * RadDeg;
        }

        // TRIGONOMETRY RADIAN

        ///<summary>sine sinA = opposite/hypotenuse = a/c [радиан]</summary>
        public static float RadSin(float f)
        {
            return Mathf.Sin(f);
        }

        ///<summary>cosine cosA = adjacent/hypotenuse = b/c [радиан]</summary>
        public static float RadCos(float f)
        {
            return Mathf.Cos(f);
        }

        ///<summary>tangent tanA = opposite/adjacent = a/b = (a/c)/(b/c) = sinA/cosA [радиан]</summary>
        public static float RadTan(float f)
        {
            return Mathf.Tan(f);
        }

        ///<summary>cosecant cscA = 1/sinA = hypotenuse/opposite = c/a [радиан]</summary>
        public static float RadCsc(float f)
        {
            return 1f / Mathf.Sin(f);
        }

        ///<summary>secant secA = 1/cosA = hypotenuse/adjacent = c/b [радиан]</summary>
        public static float RadSec(float f)
        {
            return 1f / Mathf.Cos(f);
        }

        ///<summary>cotangent cotA = 1/tanA = adjacent/opposite = cosA/sinA = b/a [радиан]</summary>
        public static float RadCot(float f)
        {
            return 1f / Mathf.Tan(f);
        }

        ///<summary>arcsine y = arcsin(x), x = sin(y) [радиан]</summary>
        public static float RadAsin(float f)
        {
            return Mathf.Asin(f);
        }

        ///<summary>arccosine y = arccos(x), x = cos(y) [радиан]</summary>
        public static float RadAcos(float f)
        {
            return Mathf.Acos(f);
        }

        ///<summary>arctangent y = arctan(x), x = tan(y) [радиан]</summary>
        public static float RadAtan(float f)
        {
            return Mathf.Atan(f);
        }

        ///<summary>arccosecant y = arccsc(x), x = csc(y) [радиан]</summary>
        public static float RadAcsc(float f)
        {
            return Mathf.Asin(1f / f);
        }

        ///<summary>arcsecant y = arcsec(x), x = sec(y) [радиан]</summary>
        public static float RadAsec(float f)
        {
            return Mathf.Acos(1f / f);
        }

        ///<summary>arccotangent y = arccot(x), x = cot(y) [радиан]</summary>
        public static float RadAcot(float f)
        {
            return Mathf.Atan(1f / f);
        }

        ///<summary>tan-н өнцөг [радиан]</summary>
        public static float RadAtan2(float y, float x)
        {
            return Mathf.Atan2(y, x);
        }
        public static Vector2 AngleToDir(float angRad)
        {
            return new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad));
        }
        public static float DirToAng(Vector2 v)
        {
            return Mathf.Atan2(v.y, v.x);
        }
        public static float eulerAngleAsNegative(float angle)
        {
            float negAngle = (angle > 180) ? angle - 360 : angle;
            return negAngle;
        }
        public static Vector3 RotateVector90(Vector3 vector)
        {
            return new Vector3(-vector.y, vector.x, vector.z);
        }
    }
}