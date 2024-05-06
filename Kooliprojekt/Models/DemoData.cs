using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;

namespace KooliProjekt.Models
{
    public class DemoData
    {
        public static void Initialize(ApplicationDbContext context, IScheduleService scheduleService)
        {

            if (context.Artists.Count() > 0)
            {
                return;
            }

            var artists = new Artist[]
               {
                    new Artist
                    {
                        Name = "John",
                        Description = "mi in nulla posuere sollicitudin aliquam ultrices sagittis orci a scelerisque purus semper eget duis",
                    },
                    new Artist
                    {
                        Name = "Ben",
                        Description = "neque gravida in fermentum et sollicitudin ac orci phasellus"
                    },
                    new Artist
                    {
                        Name = "Mike",
                        Description = "morbi blandit cursus risus at ultrices mi tempus imperdiet nulla malesuada pellentesque elit eget gravida cum sociis natoque penatibus et"
                    },
                    new Artist
                    {
                        Name = "Felix",
                        Description = "sed odio morbi quis commodo"
                    },
                    new Artist
                    {
                        Name = "Tom",
                        Description = "mi in nulla posuere sollicitudin aliquam ultrices sagittis orci a scelerisque purus semper eget duis",
                    },
                    new Artist
                    {
                        Name = "Tyson",
                        Description = "neque gravida in fermentum et sollicitudin ac orci phasellus"
                    },
                    new Artist
                    {
                        Name = "Jason",
                        Description = "morbi blandit cursus risus at ultrices mi tempus imperdiet nulla malesuada pellentesque elit eget gravida cum sociis natoque penatibus et"
                    },
                    new Artist
                    {
                        Name = "Marcus",
                        Description = "sed odio morbi quis commodo"
                    }
               };
            foreach (Artist a in artists)
            {
                context.Artists.Add(a);
            }
            context.SaveChanges();

            var songs = new Song[]
            {
                new Song
                {
                    Title = "title1",
                    Tempo = 2,
                    ArtistId = artists.Single(i => i.Name == "John").ArtistId
                },
                new Song
                {
                    Title = "2title",
                    Tempo = 1,
                    ArtistId = artists.Single(i => i.Name == "Ben").ArtistId
                },
                new Song
                {
                    Title = "Legends of Goblin",
                    Tempo = 3,
                    ArtistId = artists.Single(i => i.Name == "Mike").ArtistId
                },
                new Song
                {
                    Title = "Sad life of Boomer",
                    Tempo = 3,
                    ArtistId = artists.Single(i => i.Name == "Ben").ArtistId
                },
                new Song
                {
                    Title = "Felix's adventures",
                    Tempo = 2,
                    ArtistId = artists.Single(i => i.Name == "Felix").ArtistId
                },
                new Song
                {
                    Title = "Tom's adventures",
                    Tempo = 1,
                    ArtistId = artists.Single(i => i.Name == "Tom").ArtistId
                },
                new Song
                {
                    Title = "Tyson's adventures",
                    Tempo = 3,
                    ArtistId = artists.Single(i => i.Name == "Tyson").ArtistId
                },
                new Song
                {
                    Title = "Jason's adventures",
                    Tempo = 3,
                    ArtistId = artists.Single(i => i.Name == "Jason").ArtistId
                },
                new Song
                {
                    Title = "Marcus's adventures",
                    Tempo = 2,
                    ArtistId = artists.Single(i => i.Name == "Marcus").ArtistId
                },
                new Song
                {
                    Title = "Wonderful Train",
                    Tempo = 1,
                    ArtistId = 1
                },
                new Song
                {
                    Title = "Only monday",
                    Tempo = 1,
                    ArtistId = 8
                },
                new Song
                {
                    Title = "Tea December",
                    Tempo = 2,
                    ArtistId = 8
                },
                new Song
                {
                    Title = "Cheerful River",
                    Tempo = 2,
                    ArtistId = 7
                },
                new Song
                {
                    Title = "You Are Sinking In My Season",
                    Tempo = 2,
                    ArtistId = 7
                },
                new Song
                {
                    Title = "Always Door",
                    Tempo = 2,
                    ArtistId = 6
                },
                new Song
                {
                    Title = "Rest Morning",
                    Tempo = 2,
                    ArtistId = 6
                },
                new Song
                {
                    Title = "Beaming Nightmare",
                    Tempo = 1,
                    ArtistId = 5
                }
            };
            foreach (Song s in songs)
            {
                context.Songs.Add(s);
            }
            context.SaveChanges();

            var schedules = new Schedule[]
            {
                new Schedule{ Date = DateTime.Now, Songs = scheduleService.GetSongSchedules(DateTime.Now) },
                new Schedule{ Date = DateTime.Now.AddDays(1), Songs = scheduleService.GetSongSchedules(DateTime.Now.AddDays(1))},
                new Schedule{ Date = DateTime.Now.AddDays(-1), Songs = scheduleService.GetSongSchedules(DateTime.Now.AddDays(-1))}
            };
            foreach (var schedule in schedules)
            {
                context.Schedules.Add(schedule);
            }
            context.SaveChanges();

            

            Random random = new Random();
            var storages = new List<Storage>();
            string code;
            string current = "AAA00";

            foreach (var song in songs)
            {
                code = CodeGenerator.codeGenerator(current);
                storages.Add(new Storage { Kood = code, SongId = song.SongId });
                current = code;
            }

            foreach (Storage s in storages)
            {
                context.Storages.Add(s);
            }
            context.SaveChanges();

            
        }

    }
}
