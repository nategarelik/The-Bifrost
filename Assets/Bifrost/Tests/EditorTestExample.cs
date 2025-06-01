using NUnit.Framework;

namespace Bifrost.Tests
{
    public class EditorTestExample
    {
        [Test]
        public void PlaceholderTest()
        {
            Assert.Pass("This is a placeholder test. Replace with real tests.");
        }

        [Test]
        public void ValidatePlan_WithScriptableObject_Succeeds()
        {
            var plan = new Bifrost.Editor.GameSystemPlan
            {
                ScriptableObjects = new System.Collections.Generic.List<Bifrost.Editor.PlannedScriptableObject>
                {
                    new Bifrost.Editor.PlannedScriptableObject { Path = "Assets/TestSO.asset", TypeName = "UnityEngine.ScriptableObject", JsonData = null }
                }
            };
            string error;
            Assert.IsTrue(Bifrost.Editor.GameSystemPlan.Validate(plan, out error), error);
        }

        [Test]
        public void ValidatePlan_InvalidScriptableObjectPath_Fails()
        {
            var plan = new Bifrost.Editor.GameSystemPlan
            {
                ScriptableObjects = new System.Collections.Generic.List<Bifrost.Editor.PlannedScriptableObject>
                {
                    new Bifrost.Editor.PlannedScriptableObject { Path = "BadPath/TestSO.asset", TypeName = "UnityEngine.ScriptableObject", JsonData = null }
                }
            };
            string error;
            Assert.IsFalse(Bifrost.Editor.GameSystemPlan.Validate(plan, out error));
            Assert.IsTrue(error.Contains("Invalid ScriptableObject path"));
        }
    }
}