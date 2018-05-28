﻿using System;
using System.Threading.Tasks;

namespace Pipelines.Implementations
{
    public abstract class PostProcessorConcept : IProcessor
    {
        public static readonly string ActualProcessorMustBeProvided =
            "Creating a post processor, you have to provide an actual processor, after which an action will be executed.";

        public static readonly string ActualProcessorMustBeProvidedInGeneric =
            "Creating a generic post processor, you have to provide an actual processor, after which an action will be executed.";

        public IProcessor ActualProcessor { get; }

        protected PostProcessorConcept(IProcessor actualProcessor)
        {
            ActualProcessor = actualProcessor ?? throw new ArgumentNullException(
                                  PostProcessorConcept.ActualProcessorMustBeProvided);
        }

        public async Task Execute(object arguments)
        {
            await this.ActualProcessor.Execute(arguments);
            await this.CustomExecute(arguments);
        }

        public abstract Task CustomExecute(object arguments);
    }

    public abstract class PostProcessorConcept<T> : SafeTypeProcessor<T>
    {
        public IProcessor ActualProcessor { get; }

        protected PostProcessorConcept(IProcessor actualProcessor)
        {
            ActualProcessor = actualProcessor ?? throw new ArgumentNullException(
                            PostProcessorConcept.ActualProcessorMustBeProvidedInGeneric);
        }

        public override async Task SafeExecute(T arguments)
        {
            await this.ActualProcessor.Execute(arguments);
            await this.CustomExecute(arguments);
        }

        public abstract Task CustomExecute(T arguments);
    }
}