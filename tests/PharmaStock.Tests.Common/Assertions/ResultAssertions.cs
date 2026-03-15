using FluentAssertions;
using FluentAssertions.Execution;
using PharmaStock.BuildingBlocks.Common;

namespace PharmaStock.Tests.Common.Assertions;

public static class ResultAssertions
{
    public static ResultAssertion Should(this Result instance) =>
        new(instance);

    public static ResultAssertion<T> Should<T>(this Result<T> instance) =>
        new(instance);
}

public class ResultAssertion
{
    private readonly Result _subject;

    public ResultAssertion(Result subject) => _subject = subject;

    public AndConstraint<ResultAssertion> BeSuccess(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(_subject.IsSuccess)
            .FailWith("Expected Result to be success{reason}, but it failed with error: {0}.", _subject.Error);

        return new AndConstraint<ResultAssertion>(this);
    }

    public AndConstraint<ResultAssertion> BeFailure(string? message = null, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(!_subject.IsSuccess)
            .FailWith("Expected Result to be failure{reason}, but it succeeded.");

        if (message is not null)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(_subject.Error == message)
                .FailWith("Expected Result error to be {0}{reason}, but found {1}.", message, _subject.Error);
        }

        return new AndConstraint<ResultAssertion>(this);
    }
}

public class ResultAssertion<T>
{
    private readonly Result<T> _subject;

    public ResultAssertion(Result<T> subject) => _subject = subject;

    public AndConstraint<ResultAssertion<T>> BeSuccess(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(_subject.IsSuccess)
            .FailWith("Expected Result to be success{reason}, but it failed with error: {0}.", _subject.Error);

        return new AndConstraint<ResultAssertion<T>>(this);
    }

    public AndConstraint<ResultAssertion<T>> BeSuccessWithValue(T value, string because = "", params object[] becauseArgs)
    {
        BeSuccess(because, becauseArgs);
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(EqualityComparer<T>.Default.Equals(_subject.Value, value))
            .FailWith("Expected Result value to be {0}{reason}, but found {1}.", value, _subject.Value);

        return new AndConstraint<ResultAssertion<T>>(this);
    }

    public AndConstraint<ResultAssertion<T>> BeFailure(string? message = null, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(!_subject.IsSuccess)
            .FailWith("Expected Result to be failure{reason}, but it succeeded.");

        if (message is not null)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(_subject.Error == message)
                .FailWith("Expected Result error to be {0}{reason}, but found {1}.", message, _subject.Error);
        }

        return new AndConstraint<ResultAssertion<T>>(this);
    }
}
