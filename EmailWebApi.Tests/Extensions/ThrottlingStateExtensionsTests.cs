using System.Collections;
using System.Collections.Generic;
using EmailWebApi.Db.Entities.Dto;
using EmailWebApi.Extensions;
using Xunit;

namespace EmailWebApi.Tests.Extensions
{
    public class ThrottlingStateExtensionsTests
    {
        [Theory]
        [ClassData(typeof(ThrottlingStateDtoGenerator))]
        public void Clear(ThrottlingStateDto state)
        {
            //Act
            state.Clear();

            //Assert
            Assert.Equal(0, state.Counter);
            Assert.Equal(string.Empty, state.LastAddress);
            Assert.Equal(0, state.LastAddressCounter);
        }
    }

    public class ThrottlingStateDtoGenerator : IEnumerable<object[]>
    {
        private readonly object[] _data;

        public ThrottlingStateDtoGenerator()
        {
            _data = new object[]
            {
                new ThrottlingStateDto
                {
                    Counter = 1,
                    LastAddress = "test",
                    LastAddressCounter = 1
                }
            };
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return _data;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}