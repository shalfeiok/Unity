using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public sealed class DomainAssemblyGuardTests
    {
        private const string DomainRoot = "Assets/Game/Runtime/Domain";

        [Test]
        public void DomainAsmdef_HasNoEngineReferences()
        {
            string asmdefPath = Path.Combine(DomainRoot, "Game.Domain.asmdef");
            Assert.True(File.Exists(asmdefPath), $"Missing asmdef: {asmdefPath}");

            string json = File.ReadAllText(asmdefPath);
            StringAssert.Contains("\"noEngineReferences\": true", json);
        }

        [Test]
        public void DomainCode_DoesNotUseUnityEngineOrUnityRandom()
        {
            var csFiles = Directory.GetFiles(DomainRoot, "*.cs", SearchOption.AllDirectories)
                .Where(path => !path.Contains("/obj/") && !path.Contains("\\obj\\"));

            foreach (var file in csFiles)
            {
                string content = File.ReadAllText(file);
                Assert.False(content.Contains("using UnityEngine"), $"UnityEngine reference found: {file}");
                Assert.False(content.Contains("UnityEngine.Random"), $"UnityEngine.Random usage found: {file}");
            }
        }
    }
}
