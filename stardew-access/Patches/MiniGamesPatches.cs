using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Minigames;

namespace stardew_access.Patches
{
    public class MiniGamesPatches
    {
        public static string grandpaStoryQuery = " ";
        public static string introQuery = " ";

        internal static void IntroPatch(Intro __instance, int ___currentState)
        {
            try
            {
                string toSpeak = " ";
                if (___currentState == 3)
                {
                    toSpeak = "Travelling to Stardew Valley bus stop";
                }
                if (___currentState == 4)
                {
                    toSpeak = "Stardew valley 0.5 miles away";
                }

                if (toSpeak != " " && introQuery != toSpeak)
                {
                    introQuery = toSpeak;
                    MainClass.ScreenReader.Say(toSpeak, false);
                    return;
                }
            }
            catch (System.Exception e)
            {
                MainClass.ErrorLog($"Unable to narrate Text:\n{e.Message}\n{e.StackTrace}");
            }
        }

        internal static void GrandpaStoryPatch(GrandpaStory __instance, StardewValley.Menus.LetterViewerMenu ___letterView, bool ___drawGrandpa, bool ___letterReceived, bool ___mouseActive, Queue<string> ___grandpaSpeech, int ___grandpaSpeechTimer, int ___totalMilliseconds, int ___scene, int ___parallaxPan)
        {
            try
            {
                int x = Game1.getMouseX(true), y = Game1.getMouseY(true); // Mouse x and y position
                string toSpeak = " ";
                MainClass.DebugLog("" + ___scene);

                if (___letterView != null)
                {
                    DialoguePatches.NarrateLetterContent(___letterView);
                }

                if (MainClass.ModHelper == null)
                    return;

                if (___scene == 0)
                {
                    toSpeak = MainClass.ModHelper.Translation.Get("grandpastory.scene0");
                }

                if (___drawGrandpa)
                {
                    if (___grandpaSpeech.Count > 0 && ___grandpaSpeechTimer > 3000)
                    {
                        toSpeak = ___grandpaSpeech.Peek();
                    }
                }
                if (___scene == 3)
                {
                    toSpeak = Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12059");
                }

                if (___scene == 4)
                {
                    toSpeak = MainClass.ModHelper.Translation.Get("grandpastory.scene4");
                }
                if (___scene == 5)
                {
                    toSpeak = MainClass.ModHelper.Translation.Get("grandpastory.scene5");
                }

                if (___scene == 6)
                {
                    if (___grandpaSpeechTimer > 3000)
                    {
                        if (clickableGrandpaLetterRect(___parallaxPan, ___grandpaSpeechTimer).Contains(x, y))
                        {
                            toSpeak = MainClass.ModHelper.Translation.Get("grandpastory.letteropen");
                        }
                        else if (___letterView == null)
                        {
                            Point pos = clickableGrandpaLetterRect(___parallaxPan, ___grandpaSpeechTimer).Center;
                            Game1.setMousePositionRaw((int)((float)pos.X * Game1.options.zoomLevel), (int)((float)pos.Y * Game1.options.zoomLevel));
                            return;
                        }
                    }
                    else
                    {
                        toSpeak = MainClass.ModHelper.Translation.Get("grandpastory.scene6");
                    }
                }

                if (toSpeak != " " && grandpaStoryQuery != toSpeak)
                {
                    grandpaStoryQuery = toSpeak;
                    MainClass.ScreenReader.Say(toSpeak, false);
                }
            }
            catch (System.Exception e)
            {
                MainClass.ErrorLog($"Unable to narrate Text:\n{e.Message}\n{e.StackTrace}");
            }
        }

        // This method is taken from the game's source code
        private static Rectangle clickableGrandpaLetterRect(int ___parallaxPan, int ___grandpaSpeechTimer)
        {
            return new Rectangle((int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.viewport, 1294, 730).X + (286 - ___parallaxPan) * 4, (int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.viewport, 1294, 730).Y + 218 + Math.Max(0, Math.Min(60, (___grandpaSpeechTimer - 5000) / 8)), 524, 344);
        }
    }
}