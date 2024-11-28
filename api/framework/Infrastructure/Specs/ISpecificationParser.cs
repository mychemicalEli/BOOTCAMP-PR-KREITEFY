namespace framework.Infrastructure.Specs;

public interface ISpecificationParser<T>
{
    Specification<T> ParseSpecification(string filter);
}