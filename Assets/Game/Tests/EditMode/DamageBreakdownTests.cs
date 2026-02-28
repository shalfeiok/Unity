using Game.Domain.Modifiers;
using Game.Domain.Stats;
using Game.Domain.Tags;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public sealed class DamageBreakdownTests
    {
        private static readonly TagId AnyScope = new(0);

        [Test]
        public void DamageBreakdown_ReportsExpectedStages()
        {
            var sheet = new StatSheet();
            sheet.SetBase(StatId.Damage, 100f);
            sheet.AddModifier(new Modifier(StatId.Damage, ModifierBucket.Add, 50f, 1, AnyScope));
            sheet.AddModifier(new Modifier(StatId.Damage, ModifierBucket.Increased, 0.5f, 2, AnyScope));
            sheet.AddModifier(new Modifier(StatId.Damage, ModifierBucket.More, 0.2f, 3, AnyScope));

            var breakdown = sheet.GetDamageBreakdown(StatId.Damage);

            Assert.AreEqual(100f, breakdown.Base, 0.001f);
            Assert.AreEqual(50f, breakdown.Added, 0.001f);
            Assert.AreEqual(0.5f, breakdown.Increased, 0.001f);
            Assert.AreEqual(1.2f, breakdown.More, 0.001f);
            Assert.AreEqual(270f, breakdown.BeforeConversion, 0.001f);
            Assert.AreEqual(270f, breakdown.Final, 0.001f);
            Assert.AreEqual(0f, breakdown.ConvertedOut, 0.001f);
            Assert.AreEqual(0f, breakdown.GainedAsExtra, 0.001f);
        }

        [Test]
        public void DamageBreakdown_TracksConvertedOutAmount()
        {
            var sheet = new StatSheet();
            sheet.SetBase(StatId.Damage, 100f);
            sheet.AddModifier(new Modifier(StatId.Damage, ModifierBucket.Conversion, 0.25f, 10, AnyScope, StatId.ResistFire));

            var breakdown = sheet.GetDamageBreakdown(StatId.Damage);

            Assert.AreEqual(75f, breakdown.Final, 0.001f);
            Assert.AreEqual(25f, breakdown.ConvertedOut, 0.001f);
        }

        [Test]
        public void DamageBreakdown_AppliesGainAsExtra_CritAndMitigation()
        {
            var sheet = new StatSheet();
            sheet.SetBase(StatId.Damage, 200f);
            sheet.AddModifier(new Modifier(StatId.Damage, ModifierBucket.GainAsExtra, 0.25f, 20, AnyScope));

            var breakdown = sheet.GetDamageBreakdown(StatId.Damage, critChance: 0.5f, critMultiplier: 2f, mitigation: 0.2f);

            Assert.AreEqual(50f, breakdown.GainedAsExtra, 0.001f);
            Assert.AreEqual(250f, breakdown.AfterConversionAndExtra, 0.001f);
            Assert.AreEqual(1.5f, breakdown.CritExpectedMultiplier, 0.001f);
            Assert.AreEqual(375f, breakdown.AfterCrit, 0.001f);
            Assert.AreEqual(300f, breakdown.Final, 0.001f);
        }
    }
}
