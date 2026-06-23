using System.Numerics;
using System.Runtime.CompilerServices;

namespace RvB.EC.Shared;

public static class MathEx {
    public static T Abs<T>(this T value) where T : INumber<T> {
        return T.Abs(value);
    }

    public static int Sign<T>(this T value) where T : INumber<T> {
        return T.Sign(value);
    }

    public static T Min<T>(ReadOnlySpan<T> values) where T : INumber<T> {
        if (values.Length == 0)
            throw new ArgumentNullException(nameof(values));
        var min = values[0];
        for (var i = 1; i < values.Length; i++) {
            min = T.Min(min, values[i]);
        }
        return min;
    }

    public static T Max<T>(ReadOnlySpan<T> values) where T : INumber<T> {
        if (values.Length == 0)
            throw new ArgumentNullException(nameof(values));
        var max = values[0];
        for (var i = 1; i < values.Length; i++) {
            max = T.Max(max, values[i]);
        }
        return max;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Modulo<T>(this T number, T divisor) where T : INumber<T>
        => ((number %= divisor).CompareTo(T.Zero) < 0) ? number + divisor : number;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetDigitCount(this int n) {
        return int.Abs(n) switch {
            < 10 => 1,
            < 100 => 2,
            < 1000 => 3,
            < 10000 => 4,
            < 100000 => 5,
            < 1000000 => 6,
            < 10000000 => 7,
            < 100000000 => 8,
            < 1000000000 => 9,
            _ => 10
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetDigitCount(this long n) {
        return long.Abs(n) switch {
            < 10L => 1,
            < 100L => 2,
            < 1000L => 3,
            < 10000L => 4,
            < 100000L => 5,
            < 1000000L => 6,
            < 10000000L => 7,
            < 100000000L => 8,
            < 1000000000L => 9,
            < 10000000000L => 10,
            < 100000000000L => 11,
            < 1000000000000L => 12,
            < 10000000000000L => 13,
            < 100000000000000L => 14,
            < 1000000000000000L => 15,
            < 10000000000000000L => 16,
            < 100000000000000000L => 17,
            < 1000000000000000000L => 18,
            _ => 19
        };
    }

    public static long Pow(long num, int exp) {
        var result = 1L;
        while (exp > 0) {
            if ((exp & 1) == 1) {
                result *= num;
            }
            num *= num;
            exp >>= 1;
        }
        return result;
    }

    //From: https://en.wikipedia.org/wiki/Modular_exponentiation
    public static long ModularPow(long @base, long exponent, long modulus) {
        if (modulus == 1) {
            return 0;
        }
        Int128 res = 1;
        Int128 b = @base;
        b %= modulus;
        while (exponent > 0) {
            if ((exponent & 1) == 1) {
                res = (res * b) % modulus;
            }
            exponent >>= 1;
            b = (b * b) % modulus;
        }
        return long.CreateChecked(res);
    }

    public static T GCD<T>(T a, T b) where T : INumber<T> {
        while (b != T.Zero) {
            (a, b) = (b, a % b);
        }
        return a;
    }

    public static T LCM<T>(T a, T b) where T : INumber<T> {
        return (a * b) / GCD(a, b);
    }

    public static T InverseModulo<T>(T a, T m) where T : INumber<T> {
        T m0 = m;
        T x0 = T.Zero, x1 = T.One;

        if (m == T.One)
            return T.Zero;

        // Apply extended Euclid's Algorithm
        while (a > T.One) {
            // q is quotient
            var q = a / m;
            (a, m) = (m, a % m);
            (x0, x1) = (x1 - q * x0, x0);
        }
        // Make sure x1 is positive  
        if (x1 < T.Zero)
            x1 += m0;
        return x1;
    }

    /// <summary>
    /// Implementation of the Chinese Remainder Theorem algorithm
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pairs"></param>
    /// <returns></returns>
    public static T ChineseRemainderTheorem<T>(ReadOnlySpan<(T Divisor, T Remainder)> pairs) where T : INumber<T> {
        // Compute product of all numbers  
        T prod = T.MultiplicativeIdentity;
        for (var i = 0; i < pairs.Length; i++)
            prod *= pairs[i].Divisor;

        // Initialize result  
        T result = T.Zero;

        // Apply above formula  
        for (int i = 0; i < pairs.Length; i++) {
            var pp = prod / pairs[i].Divisor;
            result += pairs[i].Remainder * InverseModulo(pp, pairs[i].Divisor) * pp;
        }
        return result % prod;
    }

    /// <summary>
    /// Get the integer factors of <paramref name="number"/> including <typeparamref name="T"/>.One and <paramref name="number"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="number"></param>
    /// <returns></returns>
    public static IEnumerable<T> GetFactors<T>(T number) where T : IBinaryInteger<T> {
        return GetFactors(number, false);
    }

    /// <summary>
    /// Get the integer factors of <paramref name="number"/> excluding <typeparamref name="T"/>.One and <paramref name="number"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="number"></param>
    /// <returns></returns>
    public static IEnumerable<T> GetProperFactors<T>(T number) where T : IBinaryInteger<T> {
        return GetFactors(number, true);
    }

    /// <summary>
    /// Get the factor count of <paramref name="number"/> including <typeparamref name="T"/>.One and <paramref name="number"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int GetFactorCount<T>(T number) where T : IBinaryInteger<T> {
        return GetFactorCount(number, false);
    }

    /// <summary>
    /// Get the factor count of <paramref name="number"/> excluding <typeparamref name="T"/>.One and <paramref name="number"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int GetProperFactorCount<T>(T number) where T : IBinaryInteger<T> {
        return GetFactorCount(number, true);
    }

    private static IEnumerable<T> GetFactors<T>(T number, bool proper) where T : IBinaryInteger<T> {
        number = T.Abs(number);
        if (number == T.Zero)
            yield break;
        if (number == T.One) {
            yield return T.One;
            yield break;
        }
        if (!proper)
            yield return T.One;
        var factors = new List<T>();
        T i = T.One;
        i++;
        while (i * i <= number) {
            var (d, r) = T.DivRem(number, i);
            if (r == T.Zero) {
                yield return i;
                if (d != i)
                    factors.Add(d);
            }
            i++;
        }
        for (int j = factors.Count - 1; j >= 0; --j)
            yield return factors[j];
        if (!proper)
            yield return number;
    }

    private static int GetFactorCount<T>(T number, bool proper) where T : IBinaryInteger<T> {
        number = T.Abs(number);
        if (number == T.Zero)
            return 0;
        if (number == T.One)
            return 1;
        int factors = 0;
        T i = T.One;
        i++;
        while (i * i <= number) {
            var (d, r) = T.DivRem(number, i);
            if (r == T.Zero) {
                factors += 1;
                if (d != i)
                    factors += 1;
            }
            i++;
        }
        if (!proper)
            factors += 2;
        return factors;
    }
}
