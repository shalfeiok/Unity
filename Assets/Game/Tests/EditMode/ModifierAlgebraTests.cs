using Game.Domain.Modifiers;
using Game.Domain.Stats;
using Game.Domain.Tags;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public sealed class ModifierAlgebraTests
    {
        private static readonly TagId AnyScope = new(0);

        [Test]
        public void IncreasedModifiers_AreSummed()
        {
            var sheet = new StatSheet();
            sheet.SetBase(StatId.Damage, 100f);
            sheet.AddModifier(new Modifier(StatId.Damage, ModifierBucket.Increased, 0.2f, 10, AnyScope));
            sheet.AddModifier(new Modifier(StatId.Damage, ModifierBucket.Increased, 0.3f, 11, AnyScope));

            var result = sheet.GetFinal(StatId.Damage);

            Assert.AreEqual(150f, result, 0.001f);
        }

        [Test]
        public void MoreModifiers_AreMultiplied()
        {
            var sheet = new StatSheet();
            sheet.SetBase(StatId.Damage, 100f);
            sheet.AddModifier(new Modifier(StatId.Damage, ModifierBucket.More, 0.2f, 10, AnyScope));
            sheet.AddModifier(new Modifier(StatId.Damage, ModifierBucket.More, 0.5f, 11, AnyScope));

            var result = sheet.GetFinal(StatId.Damage);

            Assert.AreEqual(180f, result, 0.001f);
        }

        [Test]
        public void ConversionOrder_IsStableBySourceId()
        {
            var sheet = new StatSheet();
            sheet.SetBase(StatId.Damage, 100f);

            sheet.AddModifier(new Modifier(StatId.Damage, ModifierBucket.Conversion, 0.25f, 20, AnyScope, StatId.ResistFire));
            sheet.AddModifier(new Modifier(StatId.Damage, ModifierBucket.Conversion, 0.5f, 10, AnyScope, StatId.ResistCold));

            var computation = sheet.GetComputation(StatId.Damage);

            Assert.AreEqual(2, computation.OrderedConversions.Count);
            Assert.AreEqual(10, computation.OrderedConversions[0].SourceId);
            Assert.AreEqual(20, computation.OrderedConversions[1].SourceId);
            Assert.AreEqual(37.5f, computation.FinalValue, 0.001f);
            Assert.AreEqual(62.5f, computation.ConvertedOut, 0.001f);
        }
    }
}
