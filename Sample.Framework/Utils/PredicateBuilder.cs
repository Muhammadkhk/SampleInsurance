using System.Linq.Expressions;

namespace Sample.Framework.Utils;

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return param => true; }

        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> DynamicOperator<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second, string operatorStr)
        {
            var predicate = operatorStr switch
            {
                "And" => first.Compose(second, Expression.AndAlso),
                "Or" => first.Compose(second, Expression.OrElse),
                _ => first.Compose(second, Expression.AndAlso),
            };
            return predicate;
        }

    class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> map;

            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;
                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }

        public static List<T> Filter<T>
        (this List<T> source, string columnName,
            string compValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            Expression property = Expression.Property(parameter, columnName);
            Expression constant = Expression.Constant(compValue);
            Expression equality = Expression.Equal(property, constant);
            Expression<Func<T, bool>> predicate =
                Expression.Lambda<Func<T, bool>>(equality, parameter);

            Func<T, bool> compiled = predicate.Compile();
            return source.Where(compiled).ToList();
        }
}

