using ProductCatalogApi.Domain.Rules.Abstraction;
using ProductCatalogApi.Domain.Rules.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ProductCatalogApi.Domain.Rules.Dynamics
{
    public abstract class DynamicRuleAbstract : DynamicCompositeRule<FilterCriteria>, IDynamicCompositeRule<FilterCriteria>
    {
        private IReadOnlyList<FilterCriteria> _filterCriterias;

        public IRule<FilterCriteria> ConfigureFilterCriteria(IReadOnlyList<FilterCriteria> filterCriterias)
        {
            _filterCriterias = filterCriterias;
            return this;
        }

        public override Expression<Func<FilterCriteria, bool>> IsSatisfiedBy()
        {
            if (!_filterCriterias.Any()) return True();
            return _filterCriterias
                            .Select(CreateDynamicRule)
                            .Aggregate((rule, andRule) => rule.And(andRule))
                            .IsSatisfiedBy();
        }

        private IRule<FilterCriteria> CreateDynamicConditionalRule(FilterCriteria filter)
        {
            switch(filter.Operator)
            {
                case "or":
                    return CreateDynamicRule(filter.Left).Or(CreateDynamicRule(filter.Right));
                case "and":
                    return CreateDynamicRule(filter.Left).And(CreateDynamicRule(filter.Right));
                default:
                    return CreateDynamicRule(filter);
            }
        }

        private IRule<FilterCriteria> CreateDynamicRule(FilterCriteria criteria)
        {
            switch (criteria.Type)
            {
                case "equal":
                    return Equals(criteria.PropertyName, criteria.Value);
                case "inRange":
                    return InRange(criteria.PropertyName, criteria.Value, criteria.ValueTo);
                default:
                    throw new Exception("Invalid type");
            }
        }
    }
}
