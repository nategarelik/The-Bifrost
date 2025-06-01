using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Bifrost.Editor.AI.MCP;
using Newtonsoft.Json.Linq;

namespace Bifrost.Tests.Editor.MCP
{
    [TestFixture]
    public class MCPProtocolTests
    {
        private MCPToolRegistry toolRegistry;
        private MCPResourceRegistry resourceRegistry;
        private MCPProtocol protocol;

        [SetUp]
        public void Setup()
        {
            toolRegistry = new MCPToolRegistry();
            resourceRegistry = new MCPResourceRegistry();
            protocol = new MCPProtocol(toolRegistry, resourceRegistry);
        }

        [Test]
        public async Task TestInitializeProtocol()
        {
            var initParams = new MCPInitializeParams
            {
                ProtocolVersion = "2024-11-05",
                ClientInfo = new MCPClientInfo
                {
                    Name = "Test Client",
                    Version = "1.0.0"
                }
            };

            var result = await protocol.Initialize(initParams);

            Assert.IsNotNull(result);
            Assert.AreEqual("2024-11-05", result.ProtocolVersion);
            Assert.AreEqual("Bifrost Unity MCP Server", result.ServerInfo.Name);
            Assert.IsNotNull(result.Capabilities);
            Assert.IsTrue(result.Capabilities.Tools.ListChanged);
            Assert.IsTrue(result.Capabilities.Resources.ListChanged);
        }

        [Test]
        public async Task TestListTools()
        {
            var result = await protocol.ListTools();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Tools);
            Assert.Greater(result.Tools.Length, 0, "Should have registered tools");
        }

        [Test]
        public async Task TestListResources()
        {
            var result = await protocol.ListResources();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Resources);
            Assert.Greater(result.Resources.Length, 0, "Should have registered resources");
        }

        [Test]
        public async Task TestCallTool_InvalidTool()
        {
            var callParams = new MCPToolCallParams
            {
                Name = "non_existent_tool",
                Arguments = new JObject()
            };

            var result = await protocol.CallTool(callParams);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsError);
            Assert.Contains("Tool not found", result.Content[0].Text);
        }

        [Test]
        public async Task TestReadResource_InvalidResource()
        {
            var readParams = new MCPResourceReadParams
            {
                Uri = "unity://invalid/resource"
            };

            Assert.ThrowsAsync<System.ArgumentException>(async () =>
            {
                await protocol.ReadResource(readParams);
            });
        }
    }

    [TestFixture]
    public class MCPToolRegistryTests
    {
        private MCPToolRegistry registry;

        [SetUp]
        public void Setup()
        {
            registry = new MCPToolRegistry();
        }

        [Test]
        public void TestRegisterTool()
        {
            var tool = new TestTool();
            registry.RegisterTool(tool);

            Assert.IsTrue(registry.HasTool("test_tool"));
            Assert.AreEqual(tool, registry.GetTool("test_tool"));
        }

        [Test]
        public void TestUnregisterTool()
        {
            var tool = new TestTool();
            registry.RegisterTool(tool);
            registry.UnregisterTool("test_tool");

            Assert.IsFalse(registry.HasTool("test_tool"));
            Assert.IsNull(registry.GetTool("test_tool"));
        }

        [Test]
        public void TestGetAllTools()
        {
            var initialCount = registry.GetAllTools().Count();
            
            var tool = new TestTool();
            registry.RegisterTool(tool);

            var tools = registry.GetAllTools().ToList();
            Assert.AreEqual(initialCount + 1, tools.Count);
            Assert.IsTrue(tools.Any(t => t.Name == "test_tool"));
        }

        private class TestTool : MCPToolBase
        {
            public override string Name => "test_tool";
            public override string Description => "Test tool for unit tests";
            public override JObject InputSchema => new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["message"] = new JObject { ["type"] = "string" }
                }
            };

            public override Task<MCPToolCallResult> Execute(JObject arguments, MCPExecutionContext context)
            {
                return Task.FromResult(Success("Test executed"));
            }
        }
    }

    [TestFixture]
    public class MCPResourceRegistryTests
    {
        private MCPResourceRegistry registry;

        [SetUp]
        public void Setup()
        {
            registry = new MCPResourceRegistry();
        }

        [Test]
        public void TestRegisterResource()
        {
            var resource = new TestResource();
            registry.RegisterResource(resource);

            Assert.IsTrue(registry.HasResource("test://resource"));
            Assert.AreEqual(resource, registry.GetResource("test://resource"));
        }

        [Test]
        public void TestGetAllResources()
        {
            var initialCount = registry.GetAllResources().Count();
            
            var resource = new TestResource();
            registry.RegisterResource(resource);

            var resources = registry.GetAllResources().ToList();
            Assert.AreEqual(initialCount + 1, resources.Count);
            Assert.IsTrue(resources.Any(r => r.Uri == "test://resource"));
        }

        private class TestResource : MCPResourceBase
        {
            public override string Uri => "test://resource";
            public override string Name => "Test Resource";
            public override string Description => "Test resource for unit tests";

            public override Task<MCPResourceReadResult> Read(MCPResourceReadParams parameters)
            {
                return Task.FromResult(TextResult("Test resource content"));
            }
        }
    }
}