using NUnit.Framework;
using Mirle.A33.ConfigHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirle.A33.ConfigHandler.Tests
{
    [TestFixture()]
    public class ConfigHandlerTests
    {
        [SetUp()]
        public void BeforeTest()
        {

        }

        [Test()]
        public void ConfigHandler_InputEmptyFilePath_Test()
        {
            //arrange
            ConfigHandler handler = new ConfigHandler();
            string expected = @"D:\CsProject\ConfigHanlderTest.ini";

            //act
            string actual = handler.FilePath;

            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void ConfigHandler_FilePathNotExist_Test()
        {
            //arrange           
            string filePath = @"NoSuchFilePath";
            string expected = @"D:\NoSuchFilePath.ini";

            //act
            ConfigHandler handler = new ConfigHandler(filePath);
            var actual = handler.FilePath;

            Assert.AreEqual(expected, actual);          
        }

        [Test()]
        public void GetDataToString_SetupByProperty_Test()
        {
            //[SomeSection]
            //SomeKey = SomeValue

            //arrange
            ConfigHandler handler = new ConfigHandler(@"D:\CsProject\ConfigHandlerTest.ini")
            {
                SectionName = "SomeSection",
                KeyName = "SomeKey",
                DefaultValue = "SomeDefaultValue"
            };
            var expected = "SomeValue";z

            //act
            var actual = handler.GetDataToString();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void GetDataToString_SetupByParameter_Test()
        {
            //[SomeSection]
            //SomeKey = SomeValue

            //arrange
            ConfigHandler handler = new ConfigHandler(@"D:\CsProject\ConfigHandlerTest.ini");
            var expected = "SomeValue";

            //act
            var actual = handler.GetDataToString("SomeSection", "SomeKey", "SomeDefaultValue");

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void GetDataToString_NoSuchSection_Test()
        {
            //[NoSuchSection]

            //arrange
            ConfigHandler handler = new ConfigHandler(@"D:\CsProject\ConfigHandlerTest.ini")
            {
                SectionName = "NoSuchSection",
                KeyName = "SomeKey",
                DefaultValue = "SomeDefaultValue"
            };
            var expected = "SomeDefaultValue";

            //act
            var actual = handler.GetDataToString();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void GetDataToString_NoSuchKey_Test()
        {
            //[SomeSection]
            //NoSuchKey

            //arrange
            ConfigHandler handler = new ConfigHandler(@"D:\CsProject\ConfigHandlerTest.ini")
            {
                SectionName = "SomeSection",
                KeyName = "NoSuchKey",
                DefaultValue = "SomeDefaultValue"
            };
            var expected = "SomeDefaultValue";

            //act
            var actual = handler.GetDataToString();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void GetDataToString_NoSuchValue_Test()
        {
            //[SomeSection]
            //NoValueKey=

            //arrange
            ConfigHandler handler = new ConfigHandler(@"D:\CsProject\ConfigHandlerTest.ini")
            {
                SectionName = "SomeSection",
                KeyName = "NoValueKey",
                DefaultValue = "SomeDefaultValue"
            };
            var expected = "SomeDefaultValue";

            //act
            var actual = handler.GetDataToString();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void GetDataToBooleanTest()
        {
            //[SomeSection]
            //SomeKeyWithValueIsTrue = True

            //arrange
            ConfigHandler handler = new ConfigHandler(@"D:\CsProject\ConfigHandlerTest.ini");
            string SectionName = "SomeSection";
            string KeyName = "SomeKeyWithValueIsTrue";
            string DefaultValue = "False";
            var expected = true;

            //act
            var actual = handler.GetDataToBoolean(SectionName, KeyName, DefaultValue);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void SetDataFromStringTest()
        {
            //Empty section/key/value then save a new package.
            //[SomeSectionTwo]
            //SomeKeyTwo = SomeValueTwo

            //arrange
            ConfigHandler handler = new ConfigHandler(@"D:\CsProject\ConfigHandlerTest.ini");
            string SectionName = "SomeSectionTwo";
            string KeyName = "SomeKeyTwo";
            string SetValue = "SomeValueTwo";
            var expected = "SomeValueTwo";

            //act
            handler.SetDataFromString(SectionName, KeyName, SetValue);
            var actual = handler.GetDataToString(SectionName, KeyName, "GetDataFail");

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void SetDataFromBoolTest()
        {
            //Empty section/key/value then save a new package.
            //[SomeSectionTwo]
            //SomeKeyWithValueIsTrueTwo = True

            //arrange
            ConfigHandler handler = new ConfigHandler(@"D:\CsProject\ConfigHandlerTest.ini");
            string SectionName = "SomeSectionTwo";
            string KeyName = "SomeKeyWithValueIsTrueTwo";
            bool SetValueInBool = true;
            var expected = "True";

            //act
            handler.SetDataFromBool(SectionName, KeyName, SetValueInBool);
            var actual = handler.GetDataToString(SectionName, KeyName, "GetDataFail");

            //assert
            Assert.AreEqual(expected, actual);
        }        
    }
}