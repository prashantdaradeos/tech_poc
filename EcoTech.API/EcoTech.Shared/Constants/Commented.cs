/*

public interface IGenericValidator
{
    ValidationResult ValidateModel();
}



public class GenericValidator<T> : AbstractValidator<T>
{
    public GenericValidator(Action<GenericValidator<T>> configureRules)
    {
        configureRules(this);
    }
    public void AddRuleFor<TProperty>(Expression<Func<T, TProperty>> propertyExpression, Action<IRuleBuilder<T, TProperty>> rule)
    {
        rule(RuleFor(propertyExpression));
    }
}

public class Model : IGenericValidator
{
    public string MobileNumber { get; set; }
    public ValidationResult ValidateModel()
    {
        GenericValidator<Model> validator = new GenericValidator<Model>(m =>
        {
            m.AddRuleFor(x => x.MobileNumber, rule => rule.NotEmpty().
                WithMessage("Mobile Number can not be empty").
                Matches(@"^[6-9]\d{9}$").
                WithMessage("Enter 10 digit valid Mobile Number"));
        });
        return validator.Validate(this);
    }
}*/