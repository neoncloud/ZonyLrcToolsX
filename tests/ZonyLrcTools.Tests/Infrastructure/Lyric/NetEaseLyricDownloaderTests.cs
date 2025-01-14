using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;
using ZonyLrcTools.Cli.Config;
using ZonyLrcTools.Cli.Infrastructure.Lyric;

namespace ZonyLrcTools.Tests.Infrastructure.Lyric
{
    public class NetEaseLyricDownloaderTests : TestBase
    {
        private readonly ILyricDownloader _lyricDownloader;

        public NetEaseLyricDownloaderTests()
        {
            _lyricDownloader = GetService<IEnumerable<ILyricDownloader>>()
                .FirstOrDefault(t => t.DownloaderName == InternalLyricDownloaderNames.NetEase);
        }

        [Fact]
        public async Task DownloadAsync_Test()
        {
            var lyric = await _lyricDownloader.DownloadAsync("Hollow", "Janet Leon");
            lyric.ShouldNotBeNull();
            lyric.IsPruneMusic.ShouldBe(false);
        }

        [Fact]
        public async Task DownloadAsync_Issue_75_Test()
        {
            var lyric = await _lyricDownloader.DownloadAsync("Daybreak", "samfree,初音ミク");
            lyric.ShouldNotBeNull();
            lyric.IsPruneMusic.ShouldBe(false);
            lyric.ToString().Contains("惑う心繋ぎ止める").ShouldBeTrue();
        }

        [Fact]
        public async Task DownloadAsync_Issue_82_Test()
        {
            var lyric = await _lyricDownloader.DownloadAsync("シンデレラ (Giga First Night Remix)", "DECO 27 ギガP");
            lyric.ShouldNotBeNull();
            lyric.IsPruneMusic.ShouldBe(false);
        }

        [Fact]
        public async Task DownloadAsync_Issue84_Test()
        {
            var lyric = await _lyricDownloader.DownloadAsync("太空彈", "01");
            lyric.ShouldNotBeNull();
            lyric.IsPruneMusic.ShouldBe(false);
        }

        // About the new feature mentioned in issue #87.
        // Github Issue: https://github.com/real-zony/ZonyLrcToolsX/issues/87
        [Fact]
        public async Task DownloadAsync_Issue85_Test()
        {
            var lyric = await _lyricDownloader.DownloadAsync("Looking at Me", "Sabrina Carpenter");

            lyric.ShouldNotBeNull();
            lyric.IsPruneMusic.ShouldBeFalse();
            lyric.ToString().ShouldContain("你看起来失了呼吸");
        }

        [Fact]
        public async Task DownloaderAsync_Issue88_Test()
        {
            var lyric = await _lyricDownloader.DownloadAsync("茫茫草原", "姚璎格");

            lyric.ShouldNotBeNull();
        }

        [Fact]
        public async Task UnknownIssue_Test()
        {
            var lyric = await _lyricDownloader.DownloadAsync("主題歌Arrietty's Song", "Cécile Corbel");

            lyric.ShouldNotBeNull();
        }

        [Fact]
        public async Task DownloaderAsync_Issue101_Test()
        {
            var lyric = await _lyricDownloader.DownloadAsync("君への嘘", "VALSHE");
            lyric.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task DownloadAsync_Issue114_Test()
        {
            var options = ServiceProvider.GetRequiredService<IOptions<ToolOptions>>();
            options.Value.Provider.Lyric.Config.IsOnlyOutputTranslation = true;
            
            var lyric = await _lyricDownloader.DownloadAsync("Bones", "Image Dragons");
            lyric.ToString().ShouldNotContain("Gimme, gimme, gimme some time to think");
        }
    }
}