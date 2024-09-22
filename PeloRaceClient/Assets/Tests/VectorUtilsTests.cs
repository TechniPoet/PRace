using NUnit.Framework;
using UnityEngine;
using Utils;

namespace Tests
{
    public class VectorUtilsTests
    {
        [Test]
        public void WithZ_ShouldSetZComponentCorrectly()
        {
            var vector = new Vector3(1f, 2f, 3f);
            var result = vector.WithZ(10f);
            Assert.AreEqual(new Vector3(1f, 2f, 10f), result);
        }

        [Test]
        public void WithX_ShouldSetXComponentCorrectly()
        {
            var vector = new Vector3(1f, 2f, 3f);
            var result = vector.WithX(10f);
            Assert.AreEqual(new Vector3(10f, 2f, 3f), result);
        }

        [Test]
        public void WithY_ShouldSetYComponentCorrectly()
        {
            var vector = new Vector3(1f, 2f, 3f);
            var result = vector.WithY(10f);
            Assert.AreEqual(new Vector3(1f, 10f, 3f), result);
        }

        [Test]
        public void AsVector3_ShouldConvertVector2ToVector3Correctly()
        {
            var vector2 = new Vector2(1f, 2f);
            var result = vector2.AsVector3(false, 5f);
            Assert.AreEqual(new Vector3(1f, 2f, 5f), result);
        }

        [Test]
        public void AsVector3_ShouldConvertVector2ToVector3WithSwappedYZCorrectly()
        {
            var vector2 = new Vector2(1f, 2f);
            var result = vector2.AsVector3(true, 5f);
            Assert.AreEqual(new Vector3(1f, 5f, 2f), result);
        }

        [Test]
        public void AsVector2Int_ShouldRoundToNearestInt()
        {
            var vector2 = new Vector2(1.4f, 2.6f);
            var result = vector2.AsVector2Int();
            Assert.AreEqual(new Vector2Int(1, 3), result);
        }

        [Test]
        public void AsVector2Int_ShouldRoundNegativeValuesCorrectly()
        {
            var vector2 = new Vector2(-1.4f, -2.6f);
            var result = vector2.AsVector2Int();
            Assert.AreEqual(new Vector2Int(-1, -3), result);
        }

        [Test]
        public void AsVector2IntNormalized_ShouldClampBetweenNegativeOneAndOne()
        {
            var vector2 = new Vector2(2f, -3f);
            var result = vector2.AsVector2IntNormalized();
            Assert.AreEqual(new Vector2Int(1, -1), result);
        }

        [Test]
        public void AsVector2IntNormalized_ShouldHandleZeroCorrectly()
        {
            var vector2 = new Vector2(0f, 0f);
            var result = vector2.AsVector2IntNormalized();
            Assert.AreEqual(new Vector2Int(0, 0), result);
        }
    }
}