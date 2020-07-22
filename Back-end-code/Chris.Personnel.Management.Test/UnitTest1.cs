using Xunit;

namespace Chris.Personnel.Management.Test
{
    public class UnitTest1
    {
        [Fact]
        public void ShouldAdd()
        {
            //Arrange
            var sut = new Calculator();//sut-System Under Test

            //Act
            var result = sut.Add(1, 2);

            //Assert
            Assert.Equal(3, result);
        }
    }

    public class Calculator
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
