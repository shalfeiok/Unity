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

            Assert.AreEqual(50f, breakdown.Added, 0.001f);
            Assert.AreEqual(0.5f, breakdown.Increased, 0.001f);
            Assert.AreEqual(1.2f, breakdown.More, 0.001f);
            Assert.AreEqual(270f, breakdown.Final, 0.001f);
            Assert.AreEqual(0f, breakdown.ConvertedOut, 0.001f);
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
    }
}
