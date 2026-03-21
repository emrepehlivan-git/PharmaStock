using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace PharmaStock.BuildingBlocks.Common;

public static class Guard
{
    public static T AgainstNull<T>(
        [NotNull] T? value,
        [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);

        return value;
    }

    public static string AgainstNullOrWhiteSpace(
        string? value,
        [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or whitespace.", parameterName);

        return value;
    }

    public static T AgainstOutOfRange<T>(
        T value,
        T min,
        T max,
        [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            throw new ArgumentOutOfRangeException(parameterName, value, $"Value must be between {min} and {max}.");

        return value;
    }

    public static bool Against(
        bool condition,
        string message,
        [CallerArgumentExpression(nameof(condition))] string? parameterName = null)
    {
        if (condition)
            throw new ArgumentException(message, parameterName);

        return true;
    }

    public static T NotDefault<T>(
        T value,
        [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        where T : struct, IEquatable<T>
    {
        if (Equals(value, default(T)))
            throw new ArgumentException("Value cannot be default.", parameterName);

        return value;
    }

    public static int Positive(
        int value,
        bool allowZero = false,
        [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (allowZero)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(parameterName, value, "Value cannot be negative.");
        }
        else
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(parameterName, value, "Value must be positive.");
        }

        return value;
    }

    public static decimal Positive(
        decimal value,
        bool allowZero = false,
        [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (allowZero)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(parameterName, value, "Value cannot be negative.");
        }
        else
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(parameterName, value, "Value must be positive.");
        }

        return value;
    }

    public static int Negative(
        int value,
        [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (value >= 0)
            throw new ArgumentOutOfRangeException(parameterName, value, "Value cannot be positive.");

        return value;
    }

    public static decimal Negative(
        decimal value,
        [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (value >= 0)
            throw new ArgumentOutOfRangeException(parameterName, value, "Value cannot be positive.");

        return value;
    }

    public static int AgainstNegative(
        int value,
        [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(parameterName, value, "Value cannot be negative.");

        return value;
    }

    public static decimal AgainstNegative(
        decimal value,
        [CallerArgumentExpression(nameof(value))] string? parameterName = null)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(parameterName, value, "Value cannot be negative.");

        return value;
    }
}

