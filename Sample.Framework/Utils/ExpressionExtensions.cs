using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Framework.Utils
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> CombineExpressions<T>(
     Expression<Func<T, bool>> first,
     Expression<Func<T, bool>> second,
     LogicalOperator logicalOperator)
        {
            var parameter = Expression.Parameter(typeof(T));

            var firstBody = ReplaceParameter(first.Body, first.Parameters[0], parameter);
            var secondBody = ReplaceParameter(second.Body, second.Parameters[0], parameter);

            BinaryExpression combinedBody;
            if (logicalOperator == LogicalOperator.AND)
            {
                combinedBody = Expression.AndAlso(firstBody, secondBody);
            }
            else 
            {
                combinedBody = Expression.OrElse(firstBody, secondBody);
            }

            return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
        }


        private static Expression ReplaceParameter(Expression body, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            var visitor = new ParameterReplacer(oldParameter, newParameter);
            return visitor.Visit(body);
        }

        class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParameter ? _newParameter : base.VisitParameter(node);
            }
        }
    }
    public enum LogicalOperator 
    {
        [Display(Name = "AND")]
        AND,
        [Display(Name = "OR")]
        OR,
        [Display(Name = "")]
        None
    }
}
