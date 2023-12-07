﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SharedLib.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static T GetConfiguration<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            var o = serviceProvider.GetService<IOptions<T>>();
            return o is null ? throw new ArgumentNullException(nameof(T)) : o.Value;
        }
    }
}