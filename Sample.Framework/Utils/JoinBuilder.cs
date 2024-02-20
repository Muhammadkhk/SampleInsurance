﻿using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace Sample.Framework.Utils;

public static class JoinBuilder
{
    public static IQueryable CustomJoin(this IQueryable source1, string alias1, IQueryable source2, string alias2, string key1, string key2, string selector, params object[] args)
    {

        if (source1 == null) throw new ArgumentNullException("source1");

        if (alias1 == null) throw new ArgumentNullException("alias1");

        if (source2 == null) throw new ArgumentNullException("source1");

        if (alias2 == null) throw new ArgumentNullException("alias2");

        if (key1 == null) throw new ArgumentNullException("key1");

        if (key2 == null) throw new ArgumentNullException("key2");

        if (selector == null) throw new ArgumentNullException("selector");

        ParameterExpression p1 = Expression.Parameter(source1.ElementType, alias1);

        ParameterExpression p2 = Expression.Parameter(source2.ElementType, alias2);

        LambdaExpression keyLambda1 = DynamicExpressionParser.ParseLambda(new ParameterExpression[] { p1 }, null, key1, null);

        LambdaExpression keyLambda2 = DynamicExpressionParser.ParseLambda(new ParameterExpression[] { p2 }, null, key2, null);

        FixLambdaReturnTypes(ref keyLambda1, ref keyLambda2);

        LambdaExpression lambda = DynamicExpressionParser.ParseLambda(new ParameterExpression[] { p1, p2 }, null, selector, args);

        return source1.Provider.CreateQuery(

          Expression.Call(

            typeof(Queryable), "Join",

            new Type[] { source1.ElementType, source2.ElementType, keyLambda1.Body.Type, lambda.Body.Type },

           source1.Expression, source2.Expression, Expression.Quote(keyLambda1), Expression.Quote(keyLambda2), Expression.Quote(lambda)

           ));

    }



    private static void FixLambdaReturnTypes(ref LambdaExpression lambda1, ref LambdaExpression lambda2)
    {

        Type type1 = lambda1.Body.Type;

        Type type2 = lambda2.Body.Type;

        if (type1 != type2)
        {

            if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(Nullable<>) && type1 == type2.GetGenericArguments()[0])
            {

                lambda1 = Expression.Lambda(Expression.Convert(lambda1.Body, type2), lambda1.Parameters.ToArray());

            }

            else
            {

                // this may fail because types are incompatible

                lambda2 = Expression.Lambda(Expression.Convert(lambda2.Body, type1), lambda2.Parameters.ToArray());

            }

        }

    }
}