using dgen.Generators;
using dgen.Services;
using System.IO.Abstractions.TestingHelpers;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;
using Xunit;
using System.Collections.Generic;
using test.mocks;
using FluentAssertions;
using dgen.Exceptions;

namespace test
{

    public class GeneratorTests
    {

        #region NamespaceTests
        [Fact]
        public void GetNamespaceRootFolder()
        {
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.buildNamespace("MyClass");

            res.Should().Be("test");
        }

        [Fact]
        public void GetNamespaceForChildFolder1()
        {
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.buildNamespace(@"MyFolder\MyClass");

            res.Should().Be("test.MyFolder");
        }

        [Fact]
        public void GetNamespaceForChildFolder2()
        {
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.buildNamespace(@"MyFolder\MySubFolder\MyClass");

            res.Should().Be("test.MyFolder.MySubFolder");
        }

        [Fact]
        public void GetNamespaceInChildFolder1()
        {
            string basePath = XFS.Path(@"C:\test\MyFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.buildNamespace(@"MyClass");

            res.Should().Be("test.MyFolder");
        }

        [Fact]
        public void GetNamespaceInChildFolder2()
        {
            string basePath = XFS.Path(@"C:\test\MyFolder\MySubFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.buildNamespace(@"MyClass");

            res.Should().Be("test.MyFolder.MySubFolder");
        }

        [Fact]
        public void GetNamespaceInChildFolder3()
        {
            string basePath = XFS.Path(@"C:\test\MyFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.buildNamespace(@"MySubFolder\MyClass");

            res.Should().Be("test.MyFolder.MySubFolder");
        }

        [Fact]
        public void GetNamespaceInChildFolder4()
        {
            string basePath = XFS.Path(@"C:\test\MyFolder\MySubFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.buildNamespace(@"MySubSub\MyClass");

            res.Should().Be("test.MyFolder.MySubFolder.MySubSub");
        }

        [Fact]
        public void GetNamespaceInParentFolder()
        {
            string basePath = XFS.Path(@"C:\test\MyFolder\MySubFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.buildNamespace(@"..\MyClass");

            res.Should().Be("test.MyFolder");
        }

        [Fact]
        public void GetNamespaceInParentParentFolder()
        {
            string basePath = XFS.Path(@"C:\test\MyFolder\MySubFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.buildNamespace(@"..\..\MyClass");

            res.Should().Be("test");
        }

        [Fact]
        public void GetNamespaceInTwoManyParentFolderThrows()
        {
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            sut.Invoking(x => x.buildNamespace(@"..\MyClass")).Should().Throw<NoParentClassException>();
        }

        [Fact]
        public void GetNamespaceInTwoManyParentFolderThrows2()
        {
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            sut.Invoking(x => x.buildNamespace(@"..\..\MyClass")).Should().Throw<NoParentClassException>();
        }
        #endregion

        #region PathTests
        [Fact]
        public void BasePath()
        {
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getPath("MyClass");

            res.Should().Be(@"C:\test");
        }

        [Fact]
        public void PathWithSubFolder()
        {
            string basePath = XFS.Path(@"C:\test\MyFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getPath("MyClass");

            res.Should().Be(@"C:\test\MyFolder");
        }

        [Fact]
        public void PathWithSubSubFolder()
        {
            string basePath = XFS.Path(@"C:\test\MyFolder\MySubFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getPath("MyClass");

            res.Should().Be(@"C:\test\MyFolder\MySubFolder");
        }

        [Fact]
        public void PathSetFolder()
        {
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getPath("MyFolder\\MyClass");

            res.Should().Be(@"C:\test\MyFolder");
        }

        [Fact]
        public void PathSetFolder2()
        {
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getPath("MyFolder\\MySubFolder\\MyClass");

            res.Should().Be(@"C:\test\MyFolder\MySubFolder");
        }

        [Fact]
        public void PathWithGoBack()
        {
            string basePath = XFS.Path(@"C:\test\MyFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getPath("..\\MyClass");

            res.Should().Be(@"C:\test");
        }

        [Fact]
        public void PathWithGoBack2()
        {
            string basePath = XFS.Path(@"C:\test\MyFolder\MySubFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getPath("..\\..\\MyClass");

            res.Should().Be(@"C:\test");
        }
        #endregion

        #region FileNameTests
        [Fact]
        public void FileInBasePath(){
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getFileName("MyClass");

            res.Should().Be(@"MyClass");
        }

        [Fact]
        public void FileWithSubFolder(){
            string basePath = XFS.Path(@"C:\test\MyFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getFileName("MyClass");

            res.Should().Be(@"MyClass");
        }

        [Fact]
        public void FileWithSubSubFolder(){
            string basePath = XFS.Path(@"C:\test\MyFolder\SubFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getFileName("MyClass");

            res.Should().Be(@"MyClass");
        }

        [Fact]
        public void FileInSubFolder(){
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getFileName("MyFolder\\MyClass");

            res.Should().Be(@"MyClass");
        }

        [Fact]
        public void FileInSubSubFolder(){
            string basePath = XFS.Path(@"C:\test");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getFileName("MyFolder\\SubFolder\\MyClass");

            res.Should().Be(@"MyClass");
        }

        [Fact]
        public void FileWithGoBack(){
            string basePath = XFS.Path(@"C:\test\MyFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getFileName("..\\MyClass");

            res.Should().Be(@"MyClass");
        }

        [Fact]
        public void FileWithGoBack2(){
            string basePath = XFS.Path(@"C:\test\MyFolder\SubFolder");
            string projPath = XFS.Path(@"C:\test\test.csproj");
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>{
                { projPath, MockFileData.NullObject}
            }, basePath);
            var pds = new ProjectDiscoveryService(fs);

            var sut = new BaseGenerator(pds, new ReporterMock());

            var res = sut.getFileName("..\\..\\MyClass");

            res.Should().Be(@"MyClass");
        }
        #endregion
    }

}