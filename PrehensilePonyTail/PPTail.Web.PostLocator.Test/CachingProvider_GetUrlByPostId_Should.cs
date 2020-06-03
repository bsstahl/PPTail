using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Web.PostLocator.Test
{
    public class CachingProvider_GetUrlByPostId_Should
    {
        [Theory]
        [InlineData("0271dd63-b142-4a67-877b-42629f64a477", "Posts\\Emitting-XML.html")]
        [InlineData("02bf7ca7-b667-45fe-86b8-5c2164aa1df7", "Posts\\Using-the-Entity-Framework-with-databases-that-have-foreign-key-relationships-that-do-not-link-to-the-primary-key-of-the-child-element.html")]
        [InlineData("04f6969d-95ba-4415-8ebe-2d40be523333", "Posts\\VBNET-20-Language-Changes-Give-You-More.html")]
        [InlineData("06189847-eb2f-4498-87e0-99c0efe69233", "Posts\\Tentative-AZGiveCamp-Organizers-Meeting-Agenda.html")]
        [InlineData("06bd7503-d4f6-46fb-9a0b-5ab002531568", "Posts\\Introduction-to-AgileExtreme-Programming.html")]
        [InlineData("0836bb99-7766-4450-9ded-fe981f771e67", "Posts\\Office-Lense28093Magic-in-a-Free-App.html")]
        [InlineData("09a68c70-e4bc-494b-852b-e067b55c00e1", "Posts\\Simplify-Your-API.html")]
        [InlineData("09a8e9d0-5295-4aa0-8306-5b4a2980ae30", "Posts\\The-Next-Old-New-way-of-Thinking-About-App-Interfaces.html")]
        [InlineData("0b6ef5a0-8796-4ade-8957-af792b706a2a", "Posts\\Exception-Handling-Block.html")]
        [InlineData("0d6e40b6-e082-4a7d-a952-8850ef1e33d8", "Posts\\Security-Problems.html")]
        [InlineData("0e48d7c1-74ef-45f1-8502-ec3577714ede", "Posts\\Will-Augmented-Reality-Finally-Make-My-Lifes-Dream-Come-True.html")]
        [InlineData("12361edd-560a-4532-bec9-53b11dc3767e", "Posts\\Sample-Using-Statement-in-VBNET-2005.html")]
        [InlineData("127542b6-45e7-4ca9-a338-5ebce5385184", "Posts\\Order-Matters-in-the-Rhino-Mocks-Fluent-Interface.html")]
        [InlineData("16ee9efb-bcb7-4972-8c4c-1790656bed7d", "Posts\\Desert-Code-Camp-IV-Another-Great-Day.html")]
        [InlineData("1ad203b0-f604-43c0-8293-aad482376f98", "Posts\\Windows-8-Store-Development-for-Enterprise-Devs.html")]
        [InlineData("1b5f1ddb-ad42-446e-b484-783712195c61", "Posts\\Use-One-Email-Alias-per-Account.html")]
        [InlineData("20380740-06e6-4b0a-82ea-2d7ae4f368bc", "Posts\\Programmers-Take-Responsibility-for-Your-Programe28099s-Output.html")]
        [InlineData("267c3074-4604-4ce3-a3ae-8edd2ffb44fb", "Posts\\Top-10-Developer-Skills.html")]
        [InlineData("29651b20-4695-4d5e-82c7-5ac564f6a5b7", "Posts\\AZGiveCamp-III-is-Oct-21st-23rd.html")]
        [InlineData("29ce92c0-ad0f-43b4-8286-228bf43b6d77", "Posts\\Ive-Accepted-a-Position-at-US-Airways.html")]
        [InlineData("2eb911d4-b484-48f6-9986-5534a1f5581f", "Posts\\Encapsulation-and-Generic-Lists.html")]
        [InlineData("2f823b30-355a-49e1-8ecf-121cff1b3547", "Posts\\Afternoon-Day-2.html")]
        [InlineData("2f979553-02ba-46ae-9aff-027ab9f8916e", "Posts\\Stay-Tuned.html")]
        [InlineData("31ea041b-f62a-4f0a-a63e-453c79435c03", "Posts\\PDC-2008-Day-1.html")]
        [InlineData("34335f38-c0b5-4ad4-b7a7-e3d3ec0dc382", "Posts\\Profile-Provider-Exception.html")]
        [InlineData("35e0b224-6e56-41d9-9ab1-69e00ea598de", "Posts\\PDC-2008.html")]
        [InlineData("36949ad5-1066-4686-81b2-ce7e5c4d8bcc", "Posts\\Code-Sample-for-My-TDD-Kickstart-Sessions.html")]
        [InlineData("36e532a9-b98b-4400-a099-f2204c340a50", "Posts\\User-Experience-Done-Better.html")]
        [InlineData("39a71c38-6692-49c9-94fe-a70aa9035078", "Posts\\PDC-2008-Day-2.html")]
        [InlineData("39f292b1-d7c4-4f4a-9e65-75ab29adbb8f", "Posts\\Day-2-Early-PM.html")]
        [InlineData("3aafd970-63fc-4772-a50c-e4500d96fbfc", "Posts\\Dictionary-of-PDC-e2809803-Terms.html")]
        [InlineData("3cc08607-5998-4b6d-a7e0-0876d4cc4f7d", "Posts\\Dev-Ignite-2-Slide-Deck-Submitted.html")]
        [InlineData("3f26d6de-511b-4b50-992a-3fdac2eb753f", "Posts\\Best-of-PDC-Phoenix-in-Tweets.html")]
        [InlineData("40678866-7f86-441f-a1e4-3bb7215c8d5d", "Posts\\The-Missing-e2809cCreate-Unit-Teste2809d-feature-in-Visual-Studio-2012.html")]
        [InlineData("4257293b-e5c1-4308-acea-340a1f31b094", "Posts\\A-Software-Developers-View-of-Dynamic-Programming.html")]
        [InlineData("496f2d62-3c89-46fc-b04d-5cf5f81fa82d", "Posts\\Remove-Any-Code-Your-Users-Dont-Care-About.html")]
        [InlineData("4b9ad089-0e5f-4ce4-81f2-0f1d4a5dfaed", "Posts\\AZGiveCamp-III-Organization-Starts-33.html")]
        [InlineData("4bc96af9-3789-4658-a678-f01cba480b13", "Posts\\Presentation-Proposal-Developer-Ignite-Phoenix.html")]
        [InlineData("52bfe53a-9e7b-4431-92b7-0d50ae7197ec", "Posts\\Desert-Code-Camp-Presentation.html")]
        [InlineData("556cde80-bc1a-412f-ab63-9b505ec49970", "Posts\\PDC-Keynote-Live-Stream.html")]
        [InlineData("567d54a3-06c2-4939-9655-6d045be217fb", "Posts\\Removing-Assemblies-from-the-GAC.html")]
        [InlineData("59574c73-c66f-498e-885b-224f52d419b5", "Posts\\Developer-Ignite-Presentation-Slide-Deck.html")]
        [InlineData("68e81aad-a521-442d-9846-9f4e894a9d60", "Posts\\Final-Day.html")]
        [InlineData("69c65f4d-352e-4e1d-92bd-79d5b8cae4b3", "Posts\\NET-Open-Source-Projects.html")]
        [InlineData("69eed32b-9eeb-43df-908d-29b3eae4a395", "Posts\\Introducing-TestHelperExtensions.html")]
        [InlineData("6b32cba3-22ad-4c34-933a-64324a8eb2b7", "Posts\\Day-1.html")]
        [InlineData("6ba711ad-9052-4ab0-bf94-a66b962d8725", "Posts\\Developer-Ignite-in-Chandler.html")]
        [InlineData("6d2efe42-e15b-45ae-bbca-f20e1beec3cf", "Posts\\Oracle-String-SQL-Query-using-a-DateTime-from-C.html")]
        [InlineData("7033a3af-acf1-49a5-877a-a66d22f175c8", "Posts\\XSL-vs-Regular-Expressions.html")]
        [InlineData("77c8094b-6a05-483c-8381-787e218075bb", "Posts\\Creating-Custom-Controls-for-ASPNET-20.html")]
        [InlineData("79730947-05cd-4621-af5d-954b114d92ff", "Posts\\Day-2-PM.html")]
        [InlineData("7babb559-7848-4372-bada-55463a717865", "Posts\\Enterprise-Library-Overview.html")]
        [InlineData("7ca6c1bf-2566-434d-8382-33680f1b0d70", "Posts\\Unit-Testing-the-Data-Tier.html")]
        [InlineData("7de3f401-e11b-4170-b1fe-d3b3f71c303d", "Posts\\TDD-Helps-Validate-Your-Tests.html")]
        [InlineData("84fe235c-8b83-438b-aa0f-6dcede58cce3", "Posts\\I-have-arrived-at-the-PDC.html")]
        [InlineData("85f50739-19ea-49c6-ad0e-86c5c4ab0c43", "Posts\\Speaking-Engagements-for-October-2015.html")]
        [InlineData("87ce8579-fb47-471a-89d0-1603011eca99", "Posts\\Dynamic-Optimization-Presentation.html")]
        [InlineData("88aceb3e-d489-4236-87d2-68d39d634a80", "Posts\\T-SQL-2005.html")]
        [InlineData("88da0d75-ea12-4eb5-ac5c-0b197dd8bc48", "Posts\\No-More-Collection-Objects.html")]
        [InlineData("8d5a0399-f8d1-4631-bd28-1cdddb0a64ac", "Posts\\Demo-Code-for-EF4Ent-Sessions.html")]
        [InlineData("8e142988-0bff-4d11-b40b-823985a5f6e1", "Posts\\Solving-DataSet-Constraint-Problems.html")]
        [InlineData("97da9a1a-2df5-4cc6-8712-a4537a47c341", "Posts\\Looking-for-Evening-Events-at-Mix11.html")]
        [InlineData("9b50d169-d967-4ee1-ba25-e97713bed19b", "Posts\\Conflict-of-Interest-YAGNI-vs-Standardization.html")]
        [InlineData("9b51fb05-53a6-44e0-bddb-fddfa1b542dc", "Posts\\Two-Features-you-Need-in-Your-Service-SLAs.html")]
        [InlineData("9cbe95f7-59e3-4fe7-9c56-687f7daaa1da", "Posts\\No-summary-of-day-3-yet.html")]
        [InlineData("9cdcc051-3acd-4300-9ad8-86786ec7e647", "Posts\\OneNote-Notebooks-remain-e2809cNot-Connectede2809d.html")]
        [InlineData("9e81f613-52ca-4108-acb4-f0f2ad3d36dc", "Posts\\nUnit-vs-VSTS.html")]
        [InlineData("9f79e2fa-5718-4d0d-ba4a-1ebdefd556f8", "Posts\\SQL-ERD-for-Membership-and-Other-ASPNET-20-Services.html")]
        [InlineData("a04cf900-bb3f-4149-bbea-146cfcd94167", "Posts\\Why-I-Am-Attending-the-Pluralsight-e2809cAlgorithms-and-Data-Structurese2809d-Webcast.html")]
        [InlineData("aaa35320-c03d-41ef-95bc-8b79614344fb", "Posts\\NET-TDD-Kickstart.html")]
        [InlineData("ab6940e3-5422-4be6-8722-028636eeac46", "Posts\\NET-20-Concerns.html")]
        [InlineData("aec1e03c-9f19-4d72-aae4-a3ebb9c1dfea", "Posts\\New-OSS-Project.html")]
        [InlineData("b103a065-2760-4f21-ab0e-c30a7fc01950", "Posts\\PDC-Day-1.html")]
        [InlineData("b1c44e28-3037-44b4-9f75-7223944d1cb0", "Posts\\Owning-Code-is-Evil.html")]
        [InlineData("b34cda30-913e-40d1-b950-a8b650aaf361", "Posts\\Entity-Framework-Inheritance.html")]
        [InlineData("b355a0d2-5465-4d99-ac67-adeae7f2d7e2", "Posts\\Day-2-Am.html")]
        [InlineData("b3dd8698-1f6b-412f-ab52-a48422b79926", "Posts\\e2809cOne-Reason-to-Changee2809d-Means-the-Code.html")]
        [InlineData("b4d0b40f-8268-4cdd-a102-fec4317f463c", "Posts\\Two-Things-I-Learned-on-Pex4Funcom-Today.html")]
        [InlineData("b999239d-2aa8-49ed-a189-c9753dd52c5a", "Posts\\AZGiveCamp-II-Announced!.html")]
        [InlineData("bbcd1ce3-22d3-47b2-90b0-02fef43a1984", "Posts\\Multiple-Inheritance-Its-Time-Should-Come-Again-Soon.html")]
        [InlineData("bce4ff0e-ab0c-4d75-a04d-4b8c207cceb5", "Posts\\Wiffle-Ball-for-Charity.html")]
        [InlineData("bd40aaeb-c0af-43e4-badb-6b95327f8e9e", "Posts\\Using-Target-Specific-Code-in-a-Portable-Library.html")]
        [InlineData("c1475e88-70c3-4c8f-b122-633cb78e77f0", "Posts\\Microsoft-Developers-and-HTML5.html")]
        [InlineData("c34168ee-706f-4c7d-9665-ffd986b6f1b6", "Posts\\Regain-Access-to-the-CreateUnitTests-Command-in-VS2012.html")]
        [InlineData("c451dcdf-72fa-4d5d-b1b0-6f04ba1f2dc8", "Posts\\Development-Posters.html")]
        [InlineData("c541f92a-a77e-4556-a13d-0b8cb70a9518", "Posts\\Use-SystemDateTimeOffset-To-Better-Handle-Time-Zones.html")]
        [InlineData("c71cbe7d-a235-4aa8-85da-4cd67d761c29", "Posts\\Server-Crash.html")]
        [InlineData("c8aadf7b-b60b-4b46-9efd-3f28f51b3c89", "Posts\\Programming-Jargon-Dictionary.html")]
        [InlineData("ca95fc20-b4f5-4e5b-90f0-59548c5f5709", "Posts\\SOAe28093Beyond-the-Buzzwords.html")]
        [InlineData("cc363ae8-3e14-4947-adfc-7bcd098eee4a", "Posts\\Using-Constraints-in-Rhino-Mocks.html")]
        [InlineData("cd8c3b37-e298-4768-88b8-c2e2de937adf", "Posts\\10-Common-ASPNET-Pitfalls.html")]
        [InlineData("ceb59f2f-55d7-447b-8470-a19d93fb29e7", "Posts\\Are-you-Ready-for-the-Next-Episode.html")]
        [InlineData("d0785b7f-ff4d-4ffd-9347-151cfef6bce0", "Posts\\Web-Parts-in-ASPNET-20.html")]
        [InlineData("d34e85e8-1ef9-4605-8e74-5bb897823816", "Posts\\Day-2-AM.html")]
        [InlineData("d464359a-64f2-40ee-9642-b507447180d0", "Posts\\Upcoming-Presentations.html")]
        [InlineData("d615e6b9-cd71-47f7-9c5b-42e834bb1201", "Posts\\Generics-Concerns.html")]
        [InlineData("d9257fea-1c85-4820-9080-4f711ba8b08e", "Posts\\Continuous-Improvement-as-a-Developer.html")]
        [InlineData("dade0093-4e08-4576-9ea3-b9fc326d6a81", "Posts\\Test-Driven-Bug-Fixes.html")]
        [InlineData("dc2a5cb7-eda4-45d0-9d22-51d5b843a3aa", "Posts\\My-Comments-to-the-FCC-on-Net-Neutrality.html")]
        [InlineData("dd43877e-c55c-4db6-b859-c148aaa2d8f5", "Posts\\Arrival.html")]
        [InlineData("dde02ea3-ba8e-4066-8dc6-9cb9dbadc450", "Posts\\Code-Analysis-Rules.html")]
        [InlineData("dfc47978-fa8c-4f3f-a75d-f2c917cb12d4", "Posts\\South-Florida-Code-Camp-2011.html")]
        [InlineData("e18ba99d-acc2-4eb8-b49c-6cce3abcb550", "Posts\\Summary-Desert-Code-Camp-2006.html")]
        [InlineData("e203ddde-e61f-4d1d-bf55-9749f9e8c3c6", "Posts\\Unit-Test-Normalization.html")]
        [InlineData("e20b651b-c0d5-40c0-a94c-29bccce46b8a", "Posts\\Annual-Scott-Guthrie-Day-in-Phoenix.html")]
        [InlineData("e3217f4e-426d-4a6e-99f0-9f00b92c7f00", "Posts\\Not-Following-API-Guidelines-Has-Impact.html")]
        [InlineData("eb66140c-73cf-4690-9b36-d42c924851a6", "Posts\\Visual-Studio-Unit-Test-Generator.html")]
        [InlineData("ebfc5816-e855-47dd-898b-8163932d85ef", "Posts\\Sample-SQL-2000-XML-Query-Courtesy-of-AE.html")]
        [InlineData("ece1a3f3-8f27-40fe-9a89-a13892032e23", "Posts\\Testing-Properties-with-Inconsistent-Accessibility.html")]
        [InlineData("ef5deb5d-71c5-4081-b4ba-433fd935dc71", "Posts\\Decorating-Partial-Class-Members-in-C.html")]
        [InlineData("f0cbf275-d47c-49f4-bdb6-bace6f73d6d7", "Posts\\Loosely-Coupled-Apps.html")]
        [InlineData("f6b7e83b-8a06-4502-9f5c-234899710422", "Posts\\Tech-Bubble-Version-20.html")]
        [InlineData("fa64c95d-3b3d-4192-8c35-86d1a64d4cda", "Posts\\Holding-the-Web-on-Your-Shoulders-With-Atlas.html")]
        [InlineData("a3ea73ac-e2cd-4448-be56-b19744cd7c57", "Posts\\Optimization-for-Developers.html")]
        [InlineData("8f4f1c21-d85d-4cb2-9454-56cb80aadaf7", "Posts\\AZGiveCamp-is-Breaking-the-Mold.html")]
        [InlineData("72788e69-f45b-4a22-a119-ff117f5c9bf3", "Posts\\A-Busy-October-and-November.html")]
        [InlineData("68269657-e0c9-430d-acd4-9b4cc211fcc7", "Posts\\Code-Coverage-Teaches-and-Protects.html")]
        public void ReturnTheCorrectUrlBasedOnThePostId(String idString, String url)
        {
            var id = Guid.Parse(idString);
            var container = (null as IServiceCollection).Create();
            var serviceProvider = container.BuildServiceProvider();
            var target = new CachingProvider(serviceProvider);
            var actual = target.GetUrlByPostId(id);
            Assert.Equal(url, actual);
        }

        [Fact]
        public void ThrowAPostNotFoundExceptionIfTheIdIsNotFound()
        {
            var id = Guid.Parse("2C662C69-4A58-4DA8-9B9F-3F417841D6BF");
            var container = (null as IServiceCollection).Create();
            var serviceProvider = container.BuildServiceProvider();
            var target = new CachingProvider(serviceProvider);
            Assert.Throws<ContentItemNotFoundException>(() => target.GetUrlByPostId(id));
        }
    }
}
