using System.Collections.Generic;
using Game.Domain.Poe.Items;
using Game.Infrastructure.DataPipeline;
using NUnit.Framework;

namespace Game.Tests.EditMode.Poe
{
    public sealed class ModPoolValidatorTests
    {
        [Test]
        public void Validate_FailsOnDuplicateIds()
        {
            var validator = new ModPoolValidator();
            var mods = new List<ModDefinition>
            {
                new() { Id = "dup", MinItemLevel = 1, MinValue = 1, MaxValue = 2 },
                new() { Id = "dup", MinItemLevel = 2, MinValue = 1, MaxValue = 2 }
            };

            bool ok = validator.Validate(mods, out var error);
            Assert.False(ok);
            Assert.IsNotEmpty(error);
        }
    }
}
