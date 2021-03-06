﻿// -----------------------------------------------------------------------
// <copyright file="TVEpisodeTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUnit.Framework;
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace Knuckleball.Tests
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestFixture]
    public class TagWritingTests
    {
        private MP4File file = null;
        private string fileCopy;

        [TestFixtureSetUp]
        public void SetUp()
        {
            CopyNewTestFile();
        }

        private void CopyNewTestFile()
        {
            string directory = TestUtilities.GetTestFileDirectory();
            string fileName = Path.Combine(directory, "TVEpisode.m4v");
            this.fileCopy = Path.Combine(directory, "TVEpisodeCopy.m4v");
            File.Copy(fileName, this.fileCopy, true);
        }

        [SetUp]
        public void Read()
        {
            this.file = MP4File.Open(fileCopy);
        }

        private void RunSetValueTest(string propertyName, object expectedPropertyValue)
        {
            // We're going to use reflection to test setting of properties, so that
            // we can avoid massive amounts of duplicated code. The reflection code
            // below is equivalent of running the below code with ".PropertyName"
            // replaced by the actual property name.
            //
            // this.file.PropertyName = expectedPropertyValue;
            // this.file.WriteTags();
            // this.file = new MP4File(fileCopy);
            // this.file.ReadTags();
            // if (expectedPropertyValue == null)
            // {
            //     Assert.IsNull(this.file.PropertyName);
            // }
            // else
            // {
            //     Assert.AreEqual(expectedPropertyValue, this.file.PropertyName);
            // }
            PropertyInfo propInfo = this.file.Tags.GetType().GetProperty(propertyName);
            if (propInfo == null)
            {
                throw new InvalidOperationException("No property on the object with the name " + propertyName);
            }

            object originalValue = propInfo.GetValue(this.file.Tags, null);
            propInfo.SetValue(this.file.Tags, expectedPropertyValue, null);

            this.file.Save();

            this.file = MP4File.Open(this.fileCopy);
            propInfo = this.file.Tags.GetType().GetProperty(propertyName);
            if (expectedPropertyValue == null)
            {
                Assert.IsNull(propInfo.GetValue(this.file.Tags, null));
            }
            else
            {
                Assert.AreEqual(expectedPropertyValue, propInfo.GetValue(this.file.Tags, null));
            }
        }

        [Test]
        public void ShouldWriteTitle()
        {
            RunSetValueTest("Title", "My New Title");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Title", null);
        }

        [Test]
        public void ShouldWriteArtist()
        {
            RunSetValueTest("Artist", "My New Artist");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Artist", null);
        }

        [Test]
        public void ShouldWriteAlbum()
        {
            RunSetValueTest("Album", "My New Album");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Album", null);
        }

        [Test]
        public void ShouldWriteAlbumArtist()
        {
            RunSetValueTest("AlbumArtist", "My New Album Artist");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("AlbumArtist", null);
        }

        [Test]
        public void ShouldWriteGrouping()
        {
            RunSetValueTest("Grouping", "My New Grouping");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Grouping", null);
        }

        [Test]
        public void ShouldWriteComposer()
        {
            RunSetValueTest("Composer", "My New Composer");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Composer", null);
        }

        [Test]
        public void ShouldWriteComment()
        {
            RunSetValueTest("Comment", "My New Comment");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Comment", null);
        }

        [Test]
        public void ShouldWriteGenre()
        {
            RunSetValueTest("Genre", "My New Genre");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Genre", null);
        }

        [Test]
        public void ShouldWriteTrackNumber()
        {
            this.CopyNewTestFile();
            this.Read();

            short? track = 5;
            RunSetValueTest("TrackNumber", track);

            // Test that we can write a null value to the tag too.
            // Note: setting TrackNumber to null implies TotalTracks is also null.
            RunSetValueTest("TrackNumber", null);
            Assert.IsNull(this.file.Tags.TotalTracks);
        }

        [Test]
        public void ShouldWriteTotalTracks()
        {
            this.CopyNewTestFile();
            this.Read();

            short? track = 12;
            RunSetValueTest("TotalTracks", track);

            // Test that we can write a null value to the tag too.
            // Note: setting TotalTracks to null implies TrackNumber is also null.
            RunSetValueTest("TotalTracks", null);
            Assert.IsNull(this.file.Tags.TrackNumber);
        }

        [Test]
        public void ShouldWriteDiscNumber()
        {
            this.CopyNewTestFile();
            this.Read();

            short? discNumber = 2;
            RunSetValueTest("DiscNumber", discNumber);

            // Test that we can write a null value to the tag too.
            // Note: setting DiscNumber to null implies TotalDiscs is also null.
            RunSetValueTest("DiscNumber", null);
            Assert.IsNull(this.file.Tags.TotalDiscs);
        }

        [Test]
        public void ShouldWriteTotalDiscs()
        {
            this.CopyNewTestFile();
            this.Read();

            short? totalDiscs = 2;
            RunSetValueTest("TotalDiscs", totalDiscs);

            // Test that we can write a null value to the tag too.
            // Note: setting TotalDiscs to null implies DiscNumber is also null.
            RunSetValueTest("TotalDiscs", null);
            Assert.IsNull(this.file.Tags.DiscNumber);
        }

        [Test]
        public void ShouldWriteTempo()
        {
            short? tempo = 120;
            RunSetValueTest("Tempo", tempo);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Tempo", null);
        }

        [Test]
        public void ShouldWriteIsCompilation()
        {
            bool? compilationValue = true;
            RunSetValueTest("IsCompilation", compilationValue);

            compilationValue = false;
            RunSetValueTest("IsCompilation", compilationValue);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("IsCompilation", null);
        }

        [Test]
        public void ShouldWriteDescription()
        {
            RunSetValueTest("Description", "My New Description");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Description", null);
        }

        [Test]
        public void ShouldWriteLongDescription()
        {
            RunSetValueTest("LongDescription", "My New Long Description");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("LongDescription", null);
        }

        [Test]
        public void ShouldWriteLyrics()
        {
            RunSetValueTest("Lyrics", "My New Lyrics");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Lyrics", null);
        }

        [Test]
        public void ShouldWriteSortName()
        {
            RunSetValueTest("SortName", "My New Sort Name");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("SortName", null);
        }

        [Test]
        public void ShouldWriteSortArtist()
        {
            RunSetValueTest("SortArtist", "My New Sort artist");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("SortArtist", null);
        }

        [Test]
        public void ShouldWriteSortAlbum()
        {
            RunSetValueTest("SortAlbum", "My New Sort Album");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("SortAlbum", null);
        }

        [Test]
        public void ShouldWriteSortAlbumArtist()
        {
            RunSetValueTest("SortAlbumArtist", "My New Sort Album Artist");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("SortAlbumArtist", null);
        }

        [Test]
        public void ShouldWriteSortComposer()
        {
            RunSetValueTest("SortComposer", "My New Sort Composer");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("SortComposer", null);
        }

        [Test]
        public void ShouldWriteSortTVShow()
        {
            RunSetValueTest("SortTVShow", "My New Sort TV Show");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("SortTVShow", null);
        }

        [Test]
        public void ShouldWriteArtwork()
        {
            Image newArtwork = Image.FromFile(Path.Combine(TestUtilities.GetTestFileDirectory(), "NewArtwork.png"));
            this.file.Tags.Artwork = newArtwork;
            this.file.Save();

            this.file.Load();
            Assert.AreEqual(1, this.file.Tags.ArtworkCount);
            Assert.AreEqual("577f318762da53a41aa57d43ba6480ea", TestUtilities.ComputeHash(this.file.Tags.Artwork, this.file.Tags.ArtworkFormat));

            this.file.Tags.Artwork = null;
            this.file.Save();
            this.file.Load();
            Assert.AreEqual(0, this.file.Tags.ArtworkCount);
            Assert.IsNull(this.file.Tags.Artwork);
        }

        [Test]
        public void ShouldWriteCopyright()
        {
            RunSetValueTest("Copyright", "My New copyright notice");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Copyright", null);
        }

        [Test]
        public void ShouldWriteEncodingTool()
        {
            RunSetValueTest("EncodingTool", "My New Encoding Tool");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("EncodingTool", null);
        }

        [Test]
        public void ShouldWriteEncodedBy()
        {
            RunSetValueTest("EncodedBy", "My New Encoded By");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("EncodedBy", null);
        }

        [Test]
        public void ShouldWriteIsPodcast()
        {
            bool? podcastValue = true;
            RunSetValueTest("IsPodcast", podcastValue);

            podcastValue = false;
            RunSetValueTest("IsPodcast", podcastValue);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("IsPodcast", null);
        }

        [Test]
        public void ShouldWriteKeywords()
        {
            RunSetValueTest("Keywords", "My New Keywords");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Keywords", null);
        }

        [Test]
        public void ShouldWriteCategory()
        {
            RunSetValueTest("Category", "My New Category");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Category", null);
        }

        [Test]
        public void ShouldWriteIsHDVideo()
        {
            bool? hdVideoValue = true;
            RunSetValueTest("IsHDVideo", hdVideoValue);

            hdVideoValue = false;
            RunSetValueTest("IsHDVideo", hdVideoValue);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("IsHDVideo", null);
        }

        [Test]
        public void ShouldWriteMediaType()
        {
            RunSetValueTest("MediaType", MediaKind.Movie);

            // Test that we can write an "unset" value to the tag too.
            RunSetValueTest("MediaType", MediaKind.NotSet);
        }

        [Test]
        public void ShouldWriteContentRating()
        {
            RunSetValueTest("ContentRating", ContentRating.Explicit);

            // Test that we can write an "unset" value to the tag too.
            RunSetValueTest("ContentRating", ContentRating.NotSet);
        }

        [Test]
        public void ShouldWriteIsGapless()
        {
            bool? gaplessValue = true;
            RunSetValueTest("IsGapless", gaplessValue);

            gaplessValue = false;
            RunSetValueTest("IsGapless", gaplessValue);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("IsGapless", null);
        }
        
        [Test]
        public void ShouldWriteMediaStoreAccount()
        {
            RunSetValueTest("MediaStoreAccount", "My New Media store account");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("MediaStoreAccount", null);
        }
        
        [Test]
        public void ShouldWriteMediaStoreAccountType()
        {
            RunSetValueTest("MediaStoreAccountType", MediaStoreAccountKind.iTunes);

            // Test that we can write an "unset" value to the tag too.
            RunSetValueTest("MediaStoreAccountType", MediaStoreAccountKind.NotSet);
        }
        
        [Test]
        public void ShouldWriteMediaStoreCountry()
        {
            RunSetValueTest("MediaStoreCountry", Country.UnitedKingdom);

            // Test that we can write an "unset" value to the tag too.
            RunSetValueTest("MediaStoreCountry", Country.None);
        }
        
        [Test]
        public void ShouldWriteContentId()
        {
            RunSetValueTest("ContentId", 120);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("ContentId", null);
        }
        
        [Test]
        public void ShouldWriteArtistId()
        {
            RunSetValueTest("ArtistId", 120);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("ArtistId", null);
        }
        
        [Test]
        public void ShouldWritePlaylistId()
        {
            RunSetValueTest("PlaylistId", 120L);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("PlaylistId", null);
        }
        
        [Test]
        public void ShouldWriteGenreId()
        {
            RunSetValueTest("GenreId", 120);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("GenreId", null);
        }
        
        [Test]
        public void ShouldWriteComposerId()
        {
            RunSetValueTest("ComposerId", 120);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("ComposerId", null);
        }
        
        [Test]
        public void ShouldWriteXid()
        {
            RunSetValueTest("Xid", "My New Xid");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("Xid", null);
        }

        [Test]
        public void ShouldWriteRatingInfo()
        {
            RatingInfo ratingInfo = new RatingInfo();
            ratingInfo.RatingSource = "mpaa";
            ratingInfo.Rating = "G";
            ratingInfo.SortValue = 100;
            this.file.Tags.RatingInfo = ratingInfo;
            this.file.Save();

            this.file.Load();
            Assert.AreEqual("mpaa", this.file.Tags.RatingInfo.RatingSource);
            Assert.AreEqual("G", this.file.Tags.RatingInfo.Rating);
            Assert.AreEqual(100, this.file.Tags.RatingInfo.SortValue);
            Assert.AreEqual(string.Empty, this.file.Tags.RatingInfo.RatingAnnotation);

            this.file.Tags.RatingInfo = null;
            this.file.Save();
            this.file.Load();
            Assert.IsNull(this.file.Tags.RatingInfo);
        }

        [Test]
        public void ShouldWriteMovieInfo()
        {
            // TODO: Break this up into standalone tests.
            MovieInfo movieInfo = new MovieInfo();
            movieInfo.Cast.Add("Jimmy Bear");
            movieInfo.Directors.Add("Orson Welles");
            movieInfo.Producers.Add("Jim Evans");
            movieInfo.Screenwriters.Add("Herman J. Manciewicz");
            movieInfo.Studio = "WonderBoy Productions";

            this.file.Tags.MovieInfo = movieInfo;
            this.file.Save();

            this.file.Load();
            Assert.That(file.Tags.MovieInfo.Cast, Is.EquivalentTo(new List<string>() { "Jimmy Bear" }));
            Assert.That(file.Tags.MovieInfo.Directors, Is.EquivalentTo(new List<string>() { "Orson Welles" }));
            Assert.That(file.Tags.MovieInfo.Producers, Is.EquivalentTo(new List<string>() { "Jim Evans" }));
            Assert.That(file.Tags.MovieInfo.Screenwriters, Is.EquivalentTo(new List<string>() { "Herman J. Manciewicz" }));
            Assert.AreEqual("WonderBoy Productions", file.Tags.MovieInfo.Studio);

            // Test can have a null part of the MovieInfo
            file.Tags.MovieInfo.RemoveCast();
            this.file.Save();
            this.file.Load();
            Assert.IsFalse(file.Tags.MovieInfo.HasCast);

            // Touch the Cast property; it should be recreated.
            // Test can have a zero-length list for the MovieInfo.
            int count = this.file.Tags.MovieInfo.Cast.Count;
            this.file.Save();
            this.file.Load();
            Assert.IsTrue(file.Tags.MovieInfo.HasCast);
            Assert.AreEqual(0, file.Tags.MovieInfo.Cast.Count);

            // Test the null case.
            this.file.Tags.MovieInfo = null;
            this.file.Save();
            this.file.Load();
            Assert.IsNull(this.file.Tags.MovieInfo);
        }

        // The following are tags unique to TV Episodes

        [Test]
        public void ShouldWriteEpisodeNumber()
        {
            RunSetValueTest("EpisodeNumber", 23);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("EpisodeNumber", null);
        }

        [Test]
        public void ShouldWriteSeasonNumber()
        {
            RunSetValueTest("SeasonNumber", 4);

            // Test that we can write a null value to the tag too.
            RunSetValueTest("SeasonNumber", null);
        }

        [Test]
        public void ShouldWriteEpisodeId()
        {
            RunSetValueTest("EpisodeId", "My New Episode ID");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("EpisodeId", null);
        }

        [Test]
        public void ShouldWriteTVShow()
        {
            RunSetValueTest("TVShow", "My New TV Show");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("TVShow", null);
        }

        [Test]
        public void ShouldWriteTVNetwork()
        {
            RunSetValueTest("TVNetwork", "My New TV Network");

            // Test that we can write a null value to the tag too.
            RunSetValueTest("TVNetwork", null);
        }
    }
}
