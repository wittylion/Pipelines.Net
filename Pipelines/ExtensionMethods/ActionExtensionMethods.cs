﻿using System;
using System.Threading.Tasks;
using Pipelines.Implementations.Processors;

namespace Pipelines.ExtensionMethods
{
    /// <summary>
    /// Extension methods for classes <see cref="Action{T}"/>
    /// and <see cref="Func{TResult}"/>.
    /// </summary>
    public static class ActionExtensionMethods
    {
        /// <summary>
        /// Converts an <see cref="Action{T}"/> to the asyncchronous
        /// function <see cref="Func{T, TResult}"/> where TResult is Task.
        /// </summary>
        /// <typeparam name="T">
        /// Parameter of the action.
        /// </typeparam>
        /// <param name="action">
        /// Represents an action to be converted to asynchronous function.
        /// </param>
        /// <returns>
        /// Returns a function which is produced from <paramref name="action"/>
        /// or <c>null</c> if action is null.
        /// </returns>
        internal static Func<T, Task> ToAsync<T>(this Action<T> action)
        {
            if (action.HasNoValue())
                return null;

            return args =>
            {
                action(args);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Converts an asynchronous function to a processor.
        /// Helps to write code quickly, without introducing
        /// instance of interface <see cref="IProcessor"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Parameter of the asynchronous function.
        /// </typeparam>
        /// <param name="action">
        /// Represents an asynchronous function to be converted to a processor.
        /// </param>
        /// <returns>
        /// Returns a processor which is produced from <paramref name="action"/>
        /// or <c>null</c> if action is null.
        /// </returns>
        public static SafeTypeProcessor<T> ToProcessor<T>(this Func<T, Task> action)
        {
            if (action.HasNoValue())
                return null;

            return ActionProcessor.FromAction<T>(action);
        }

        /// <summary>
        /// Converts an action to a processor. Helps to write code quickly,
        /// without introducing instance of interface <see cref="IProcessor"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Parameter of the action.
        /// </typeparam>
        /// <param name="action">
        /// Represents an action to be converted to a processor.
        /// </param>
        /// <returns>
        /// Returns a processor which is produced from <paramref name="action"/>
        /// or <c>null</c> if action is null.
        /// </returns>
        public static SafeTypeProcessor<T> ToProcessor<T>(this Action<T> action)
        {
            if (action.HasNoValue())
                return null;

            return ActionProcessor.FromAction<T>(action);
        }
    }
}