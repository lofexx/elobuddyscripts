using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_addon
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using SharpDX;
    using EloBuddy.SDK.Spells;
    using EloBuddy.SDK.Rendering;


    using Microsoft.Win32;





    class Program
    {
        static void Main(string[] args)
        {
            //triggers once loading is complete
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;



        }


        // Calls Player.Instance.


        private static AIHeroClient User = Player.Instance;



        //declares kata's Q as a targeted ability.
        private static Spell.Targeted Q;


        //declare kata W as a skillshot 
        private static Spell.Active W;


        //declare kata E as a targeted.
        private static Spell.Targeted E;


        //declare kata R as a active.
        private static Spell.Active R;


        //adds menu
        private static Menu KatarinaMenu, ComboMenu, DrawingsMenu;


        // A list the contain Player spells
        private static List<Spell.SpellBase> SpellList = new List<Spell.SpellBase>();


        private static void Loading_OnLoadingComplete(EventArgs args)
        {

            if (User.ChampionName != "Katarina")
            {
                return;

            }

            //main menu
            


            // submenu (combo)
            ComboMenu = KatarinaMenu.AddSubMenu("Combo");
            DrawingsMenu = KatarinaMenu.AddSubMenu("Drawings");
            // Checkbox (should be like this: YourMenu.Add(String MenuID, new CheckBox(String DisplayName, Bool DefaultValue);

            ComboMenu.Add("Q", new CheckBox("use Q"));
            ComboMenu.Add("W", new CheckBox("use W"));
            ComboMenu.Add("E", new CheckBox("use E"));
            ComboMenu.Add("R", new CheckBox("use R"));

            foreach (var Spell in SpellList)
            {
                // Creats Checkboxes using Spell Slot
                DrawingsMenu.Add(Spell.Slot.ToString(), new CheckBox("Draw " + Spell.Slot));
            }

            // values for spells
            Q = new Spell.Targeted(SpellSlot.Q, 675);
            W = new Spell.Active(SpellSlot.W, 375);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Active(SpellSlot.R, 550);




            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

          KatarinaMenu = MainMenu.AddMenu("Katarina", "Katarina");
            Drawing.OnDraw += Drawing_OnDraw; }




        private static void Drawing_OnDraw(EventArgs args)
        {
            // Returns Each spell form the list that are enabled from the menu
            foreach (var Spell in SpellList.Where(spell => DrawingsMenu[spell.Slot.ToString()].Cast<CheckBox>().CurrentValue))
            {
                // Draws a Circle with the spell Range around the Player
                Circle.Draw(Spell.IsReady() ? Color.Chartreuse : Color.OrangeRed, Spell.Range, User);
            }





            Game.OnTick += Game_OnTick;


            // Used for drawings that dosnt override Game UI
            Drawing.OnDraw += Drawing_OnDraw;














        }

        private static void Game_OnTick(EventArgs args)
        {
            // Returns true if Combo mode is Active in the orbwalker
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                // Triggers our created Combo
                Combo();
            }
        }

        private static void Combo()
        {
            
            var target = TargetSelector.GetTarget(W.Range, DamageType.Mixed);


            if (target == null)
            {
                return;
            }

            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue)
            {

                Q.Cast(target);





            }


            if (ComboMenu["W"].Cast<CheckBox>().CurrentValue)
            {

                W.Cast(target);
            
        


            }
            

            if (ComboMenu["E"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(target);


            }


            if (ComboMenu["R"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(target);

             }
            }
           }




        }
