﻿using System.Linq;
using FluentAssertions;
using Xunit;

namespace Pipelines.Tests.Units
{
    public class PipelineContextTests
    {
        [Fact]
        public void GetMessages_Returns_Information_Messages_When_All_Types_Of_Messages_Are_Added()
        {
            var information = new PipelineMessage(nameof(PipelineContext), MessageType.Information);
            var pipelineContext = new PipelineContextTestObject()
                .WithWarning(nameof(PipelineContext))
                .WithMessage(information)
                .WithError(nameof(PipelineContext));

            pipelineContext.GetMessages(MessageFilter.Informations).Should()
                .HaveCount(1, "only one information message was added")
                .And
                .AllBeEquivalentTo(information, "this message was added to collection");
        }

        [Fact]
        public void AbortMessageWithError_Calls_Abort_Pipeline_Method()
        {
            var pipelineContext = new PipelineContextTestObject();
            pipelineContext.AbortPipelineWithErrorMessage(nameof(PipelineContext));
            pipelineContext.IsAborted.Should().BeTrue("because the method should abort pipeline");
        }

        [Fact]
        public void GetInformationsAndWarnings_Should_Retrieve_Warnings_And_Informations_When_Several_Message_Are_Added()
        {
            var pipelineContext = new PipelineContextTestObject()
                .WithError("Error")
                .WithInformation("Information")
                .WithWarning("Warning");

            pipelineContext.GetInformationsAndWarnings().Should()
                .HaveCount(2, "because three messages were added, where only one of them is error")
                .And
                .Match(collection => collection.All(x => x.MessageType != MessageType.Error), "because collection should contain no Error type");
        }

        [Fact]
        public void GetPropertyValueOrNull_Retrieves_A_Proper_Value()
        {
            var pipelineContext = new PipelineContext();
            var expectedValue = nameof(GetPropertyValueOrNull_Retrieves_A_Proper_Value);
            var key = nameof(PipelineContextTests);

            pipelineContext.SetOrAddProperty(key, expectedValue);

            pipelineContext.GetPropertyValueOrNull<string>(key)
                .Should()
                .Be(expectedValue, "because method must return value when it is set");
        }

        [Fact]
        public void GetPropertyValueOrNull_Retrieves_Null_When_Requested_An_Incorrect_Type()
        {
            var pipelineContext = new PipelineContext();
            var value = nameof(GetPropertyValueOrNull_Retrieves_Null_When_Requested_An_Incorrect_Type);
            var key = nameof(PipelineContextTests);

            pipelineContext.SetOrAddProperty(key, value);

            pipelineContext.GetPropertyValueOrNull<IAsyncLifetime>(key)
                .Should()
                .BeNull("because method must check a type before value is convertedinto this type");
        }
        
        [Fact]
        public void GetPropertyValueOrDefault_Retrieves_Default_Value_When_Requested_An_Incorrect_Type()
        {
            var pipelineContext = new PipelineContext();
            var value = nameof(GetPropertyValueOrDefault_Retrieves_Default_Value_When_Requested_An_Incorrect_Type);
            var key = nameof(PipelineContextTests);
            var expectedValue = 3;

            pipelineContext.SetOrAddProperty(key, value);

            pipelineContext.GetPropertyValueOrDefault<int>(key, expectedValue)
                .Should()
                .Be(expectedValue, "because method must check a type before value is convertedinto this type");
        }

        [Fact]
        public void GetPropertyValueOrNull_Retrieves_A_Proper_Value_Regardless_Name_Case()
        {
            var pipelineContext = new PipelineContext();
            var expectedValue = nameof(GetPropertyValueOrNull_Retrieves_A_Proper_Value_Regardless_Name_Case);
            var key = nameof(PipelineContextTests);

            pipelineContext.SetOrAddProperty(key, expectedValue);

            pipelineContext.GetPropertyValueOrNull<string>(key.ToUpperInvariant())
                .Should()
                .Be(expectedValue, "because method must return value regardless case of the property name");
        }

        [Fact]
        public void ObjectConstructor_Sets_Properties_Of_An_Object_To_Property_Collection()
        {
            var expectedValue = nameof(ObjectConstructor_Sets_Properties_Of_An_Object_To_Property_Collection);
            var pipelineContext = new PipelineContext(new {PipelineContextTests = expectedValue});

            pipelineContext.GetPropertyValueOrNull<string>("PipelineContextTests")
                .Should()
                .Be(expectedValue, "because method must return value of the property passed in constructor with an object");
        }


        [Fact]
        public void ObjectConstructor_Does_Not_Throw_An_Exception_When_Object_Is_Null()
        {
            var pipelineContext = new PipelineContext(null);
            pipelineContext.Should().NotBeNull();
        }


        [Fact]
        public void GetPropertyValueOrNull_Retrieves_Null_When_Value_Is_Not_Set()
        {
            var pipelineContext = new PipelineContext();
            var expectedValue = nameof(GetPropertyValueOrNull_Retrieves_Null_When_Value_Is_Not_Set);
            var key = nameof(PipelineContextTests);

            pipelineContext.GetPropertyValueOrNull<string>(key)
                .Should()
                .BeNull(expectedValue, "because the value was not set");
        }

        [Fact]
        public void GetPropertyValueOrDefault_Retrieves_Default_Object_When_Value_Is_Not_Set()
        {
            var pipelineContext = new PipelineContext();
            var expectedValue = nameof(GetPropertyValueOrDefault_Retrieves_Default_Object_When_Value_Is_Not_Set);
            var key = nameof(PipelineContextTests);

            pipelineContext.GetPropertyValueOrDefault<string>(key, expectedValue)
                .Should()
                .Be(expectedValue, "because method must return the default passed value");
        }

        [Fact]
        public void SetOrAddProperty_Shoul_Update_Value_When_It_Has_Already_Been_Set()
        {
            var pipelineContext = new PipelineContext();
            var value = 234;
            var expectedValue = nameof(SetOrAddProperty_Shoul_Update_Value_When_It_Has_Already_Been_Set);
            var key = nameof(PipelineContextTests);

            pipelineContext.SetOrAddProperty(nameof(PipelineContextTests), value);
            pipelineContext.SetOrAddProperty(nameof(PipelineContextTests), expectedValue);
            
            pipelineContext.GetPropertyValueOrNull<string>(key)
                .Should()
                .Be(expectedValue, "because method must update a value if it previously was set");
        }

        [Fact]
        public void AddOrSkipPropertyIfExists_Shoul_Skip_Property_When_It_Has_Already_Been_Set()
        {
            var pipelineContext = new PipelineContext();
            var value = 234;
            var expectedValue = nameof(AddOrSkipPropertyIfExists_Shoul_Skip_Property_When_It_Has_Already_Been_Set);
            var key = nameof(PipelineContextTests);

            pipelineContext.AddOrSkipPropertyIfExists(nameof(PipelineContextTests), expectedValue);
            pipelineContext.AddOrSkipPropertyIfExists(nameof(PipelineContextTests), value);

            pipelineContext.GetPropertyValueOrNull<string>(key)
                .Should()
                .Be(expectedValue, "because method must skip a value if it previously was set");
        }
    }

    public class PipelineContextTestObject : PipelineContext
    {
        public virtual PipelineContextTestObject WithMessage(PipelineMessage message)
        {
            this.AddMessageObject(message);
            return this;
        }

        public virtual PipelineContextTestObject WithMessage(string message)
        {
            this.AddMessage(message);
            return this;
        }

        public virtual PipelineContextTestObject WithMessage(string message, MessageType type)
        {
            this.AddMessage(message, type);
            return this;
        }

        public virtual PipelineContextTestObject WithWarning(string message)
        {
            this.AddWarning(message);
            return this;
        }

        public virtual PipelineContextTestObject WithInformation(string message)
        {
            this.AddInformation(message);
            return this;
        }

        public virtual PipelineContextTestObject WithError(string message)
        {
            this.AddError(message);
            return this;
        }
    }
}
