using _321_Lab05_3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;

namespace pigeonthapae
{
    public class Game1 : Game
    {
        //setting
        private int bound_X = 1100;
        private int bound_Y = 600; //y start 308
        private int boss_health = 40;
        private int boss_health_per_level = 20;
        private int Time_spawn_dek = 60;
        private int bar_width = 262;
        static public int money = 20;
        private Vector2 where_dekspawn = new Vector2(-150, 300);
        private int bird_price = 10;
        private int food_price = 1;
        private int price_police = 50;
        private int price_sign = 20;
        private int price_car = 100;
        private float time_police = 30;
        private float time_sign = 5;
        private float time_car = 30;
        private float cooldown_police = 35;
        private float cooldown_sign = 10;
        private float cooldown_car = 40;
        private Vector2 area_sign = new Vector2(100, 100);

        //system
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont uid_font,cooldown_text;
        private KeyboardState _keyboardState, oldstate;
        private MouseState _mousestate, oldms, lastms;
        private Texture2D pigeon_texture, bg, kid_texure, barcolor, buy_bird, buy_car, food_texture, police_texture, sign_texture, buy_police, buy_sign;
        private Texture2D pigeon_fly, car_texture, board_ui, bar_grob, talk_texture, hit_effect;
        private List<Pigeon> bird = new List<Pigeon>();
        private List<kid> dek = new List<kid>();
        private List<food> bfood = new List<food>();
        private List<Police> _police = new List<Police>();
        private List<Sign> _sign = new List<Sign>();
        private List<Car> _car = new List<Car>();
        private List<Ceffect> hit = new List<Ceffect>();
        private float elapsed;
        private Random rnd = new Random();
        private bool attacked = false;
        private float time_pick = 0;
        private Rectangle bar, button_bird, button_sign, button_police, button_car, none_area, none_area2;
        private string type_click = "food";
        private float c_sign, c_police, c_car;
        private string holding_text, holding_price;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            none_area = new Rectangle(0, 0, 1200, 308);
            none_area2 = new Rectangle(0, 618, 1200, 600);
            button_bird = new Rectangle(373, 739, 98, 44);
            button_police = new Rectangle(650, 710, 146, 80);
            button_sign = new Rectangle(820, 698, 170, 102);
            button_car = new Rectangle(1010, 698, 190, 98);
            c_sign = cooldown_sign;
            c_police = cooldown_police;
            c_car = cooldown_car;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            pigeon_texture = Content.Load<Texture2D>("bird/bird_main");
            pigeon_fly = Content.Load<Texture2D>("bird/bird_fly");
            kid_texure = Content.Load<Texture2D>("dek/dek");
            food_texture = Content.Load<Texture2D>("bird/food");
            police_texture = Content.Load<Texture2D>("police/police");
            sign_texture = Content.Load<Texture2D>("sign");
            car_texture = Content.Load<Texture2D>("car/car");
            buy_police = Content.Load<Texture2D>("UI/police_button");
            buy_sign = Content.Load<Texture2D>("UI/sign_button");
            buy_car = Content.Load<Texture2D>("UI/car_button");
            buy_bird = Content.Load<Texture2D>("UI/bird_button");
            barcolor = Content.Load<Texture2D>("bar");
            bar_grob = Content.Load<Texture2D>("UI/bar_grob");
            board_ui = Content.Load<Texture2D>("UI/board");
            uid_font = Content.Load<SpriteFont>("textfont_uid");
            cooldown_text = Content.Load<SpriteFont>("cooldown_text");
            talk_texture = Content.Load<Texture2D>("UI/talk");
            hit_effect = Content.Load<Texture2D>("effect/hit_effect");
            bg = Content.Load<Texture2D>("bg");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            bird.Add(new Pigeon(pigeon_texture, pigeon_fly, new Vector2(rnd.Next(10, bound_X), rnd.Next(310, bound_Y))));
            bird.Add(new Pigeon(pigeon_texture, pigeon_fly, new Vector2(rnd.Next(10, bound_X), rnd.Next(310, bound_Y))));
            bird.Add(new Pigeon(pigeon_texture, pigeon_fly, new Vector2(rnd.Next(10, bound_X), rnd.Next(310, bound_Y))));
        }

        protected override void Update(GameTime gameTime)
        {
            //end game
            if (bird.Count <= 0)
            {
                //    Exit();
            }




            //time
            elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            time_pick += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //cooldown skill
            c_police -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            c_sign -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            c_car -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (c_police <= 0)
            {
                c_police = 0;
            }
            if (c_sign <= 0)
            {
                c_sign = 0;
            }
            if (c_car <= 0)
            {
                c_car = 0;
            }


            //state mouse keyboard
            _mousestate = Mouse.GetState();
            _keyboardState = Keyboard.GetState();


            //cheat
            if (_keyboardState.IsKeyDown(Keys.A) & oldstate.IsKeyUp(Keys.A))
            {
                money += 500;
            }
            if (_keyboardState.IsKeyDown(Keys.R) & oldstate.IsKeyUp(Keys.R))
            {
                c_car = 0;
                c_sign = 0;
                c_police = 0;
            }


            //debug
            if (_keyboardState.IsKeyDown(Keys.Q) & oldstate.IsKeyUp(Keys.Q))
            {
                Console.WriteLine("elapsed = " + elapsed);
                Console.WriteLine("money = " + money);
                Console.WriteLine("Time = " + time_pick);
                Console.WriteLine("X = " + _mousestate.X + " Y =" + _mousestate.Y);
                Console.WriteLine("Police Cooldown = " + c_police);
                Console.WriteLine("Sign Cooldown = " + c_sign);
                Console.WriteLine("Car Cooldown = " + c_car);
                Console.WriteLine("Type = " + type_click);

            }


            //spawn dek
            if (time_pick >= Time_spawn_dek || _keyboardState.IsKeyDown(Keys.H) & oldstate.IsKeyUp(Keys.H))
            {
                dek.Add(new kid(kid_texure, where_dekspawn, boss_health, barcolor));
                attacked = true;
            }


            //update bird
            foreach (Pigeon ppbird in bird)
            {
                ppbird.move(none_area);
                //randomlocation & check bird go outside
                ppbird.updatebird(bound_X, bound_Y, elapsed, bfood);
                if (ppbird.pos.Y <= 0 - pigeon_texture.Height)
                {
                    bird.Remove(ppbird);
                    break;
                }
                //check bird_with_food
                foreach (food mini_food in bfood)
                {
                    if (ppbird.checkfood(mini_food.hitbox))
                    {
                        bfood.Remove(mini_food);
                        break;
                    }
                }
            }


            //check attacked
            if (attacked)
            {

                time_pick = 0;
                //check all dek die
                if (dek.Count <= 0)
                {
                    attacked = false;
                    boss_health += boss_health_per_level;
                }
                //update dek , location dek, move, check_with_bird
                foreach (kid deks in dek)
                {

                    //select location & check if dek go out
                    deks.selectbird(bird, elapsed, _car, _sign, none_area);
                    if (deks.pos.X < -100 && deks.death)
                    {
                        dek.Remove(deks);
                        break;
                    }
                    deks.move();
                    //check_with_bird
                    foreach (Pigeon checkinterbird in bird)
                    {
                        checkinterbird.checkintersect(deks.hitbox);
                    }

                    //check_with_police & update
                    foreach (Police mini_police in _police)
                    {
                        deks.damaged(mini_police.hitbox, elapsed);
                    }

                }
            }
            else
            {
                bar = new Rectangle(68, 670, (int)(bar_width - (bar_width * (time_pick / Time_spawn_dek))), 10);
            }

            //change type click
            if (type_click != "sign_select")
            {
                if (button_bird.Contains(_mousestate.X, _mousestate.Y))
                {
                    type_click = "buybird";
                    holding_text = "Buy Bird";
                    holding_price = "10 Coins";
                }
                else if (button_sign.Contains(_mousestate.X, _mousestate.Y))
                {
                    type_click = "sign";
                    holding_text = "Buy Sign";
                    holding_price = "20 Coins";
                }
                else if (button_police.Contains(_mousestate.X, _mousestate.Y))
                {
                    type_click = "police";
                    holding_text = "Call Police";
                    holding_price = "50 Coins";
                }
                else if (button_car.Contains(_mousestate.X, _mousestate.Y))
                {
                    type_click = "car";
                    holding_text = "Call Red Car";
                    holding_price = "100 Coins";
                }
                else if (none_area.Contains(_mousestate.X, _mousestate.Y) || none_area2.Contains(_mousestate.X, _mousestate.Y))
                {
                    type_click = "None";
                    holding_text = " ";
                    holding_price = " ";
                }
                else if (_mousestate.X >= _graphics.GraphicsDevice.Viewport.Width || _mousestate.Y > _graphics.GraphicsDevice.Viewport.Height || _mousestate.X <= 0 || _mousestate.Y <= 0)
                {
                    type_click = "None";
                    holding_text = " ";
                    holding_price = " ";
                }
                else
                {
                    type_click = "food";
                    holding_text = " ";
                    holding_price = " ";
                }
            }
            //click screen
            if (_mousestate.LeftButton == ButtonState.Pressed && oldms.LeftButton == ButtonState.Released)
            {
                if (type_click == "food")
                {
                    if (attacked)
                    {
                        foreach (kid mini_dek in dek)
                        {
                            if (!mini_dek.damaged(_mousestate, oldms) & money >= food_price)
                            {
                                bfood.Add(new food(food_texture, 1, 1, 1, new Vector2(_mousestate.X, _mousestate.Y)));

                                money -= food_price;
                            }
                            else //feedback
                            {
                                hit.Add(new Ceffect(hit_effect, new Vector2(_mousestate.X, _mousestate.Y)));
                            }
                        }
                    }
                    else
                    {
                        if (money >= food_price)
                        {
                            bfood.Add(new food(food_texture, 1, 1, 1, new Vector2(_mousestate.X, _mousestate.Y)));
                            money -= food_price;
                        }
                    }
                }
                else if (type_click == "buybird" & money >= bird_price)
                {
                    bird.Add(new Pigeon(pigeon_texture, pigeon_fly, new Vector2(rnd.Next(10, bound_X), rnd.Next(310, bound_Y))));
                    money -= bird_price;
                }
                else if (type_click == "sign" & c_sign == 0 & money >= price_sign)
                {
                    type_click = "sign_select";
                    holding_text = "Choose the area";
                    holding_price = " ";
                    money -= price_sign;
                }
                else if (type_click == "sign_select")
                {
                    if (!none_area.Contains(_mousestate.X, _mousestate.Y) & !none_area2.Contains(_mousestate.X, _mousestate.Y) )
                    {
                        if (_mousestate.X < _graphics.GraphicsDevice.Viewport.Width & _mousestate.Y < _graphics.GraphicsDevice.Viewport.Height & _mousestate.X > 0 & _mousestate.Y > 0)
                        {
                            c_sign = cooldown_sign;
                            type_click = "food";
                            _sign.Add(new Sign(sign_texture, new Vector2(_mousestate.X - (sign_texture.Width / 2), _mousestate.Y - (sign_texture.Height / 2)), time_sign, area_sign));
                        }
                    }
                }
                else if (type_click == "police" & c_police == 0 & money >= price_police)
                {
                    _police.Add(new Police(police_texture, time_police));
                    c_police = cooldown_police;
                    money -= price_police;
                }
                else if (type_click == "car" & c_car == 0 & money >= price_car)
                {
                    _car.Add(new Car(car_texture, time_car));
                    c_car = cooldown_car;
                    money -= price_car;
                }

            }

            //update car
            foreach (Car mini_car in _car)
            {
                mini_car.update(elapsed);
                if (mini_car.pos.X < -250)
                {
                    _car.Remove(mini_car);
                    break;
                }
                foreach (kid mini_dek in dek)
                {
                    if (mini_dek.health <= mini_dek.max_health / 2 & mini_dek.hitbox.Intersects(mini_car.hitbox))
                    {
                        dek.Remove(mini_dek);
                        break;
                    }
                }
            }
            //update police

            foreach (Police mini_police in _police)
            {
                mini_police.selecttarget(dek, elapsed);
                mini_police.update();
                if (mini_police.time_out)
                {
                    if (mini_police.pos.X >= _graphics.GraphicsDevice.Viewport.Width)
                    {
                        _police.Remove(mini_police);
                        break;
                    }
                }
            }

            //update sign
            foreach (Sign mini_sign in _sign)
            {
                if (mini_sign.time_out(elapsed))
                {
                    _sign.Remove(mini_sign);
                    break;
                }
            }

            //Control worth coin
            if(bird.Count > 0 && bird.Count <= 5)
            {
                Coin.worth = 5;
            }
            else if(bird.Count > 5 && bird.Count <= 10)
            {
                Coin.worth = 4;
            }
            else if (bird.Count > 10 && bird.Count <= 15)
            {
                Coin.worth = 3;
            }
            else if (bird.Count > 15 && bird.Count <= 20)
            {
                Coin.worth = 2;
            }
            else if (bird.Count > 20)
            {
                Coin.worth = 1;
            }


            //destroy food
            foreach (food mini_food in bfood)
            {
                if (mini_food.selfdestroy(elapsed))
                {
                    bfood.Remove(mini_food);
                    break;
                }
            }

            //update effect
            foreach(Ceffect mini_effect in hit)
            {
                if(mini_effect.update(elapsed))
                {
                    hit.Remove(mini_effect);
                    break;
                }
            }


            //old item
            oldms = _mousestate;
            oldstate = _keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(bg, _graphics.GraphicsDevice.Viewport.Bounds, Color.White);
            //food
            foreach (food mini_food in bfood)
            {
                mini_food.Draw(_spriteBatch);
            }
            //sign
            foreach (Sign mini_sign in _sign)
            {
                mini_sign.Draw(_spriteBatch);
            }
            //bird
            foreach (Pigeon ppbird in bird)
            {
                ppbird.Pigeondraw(_spriteBatch, uid_font);

            }
            //police
            foreach (Police mini_police in _police)
            {
                mini_police.Draw(_spriteBatch);
            }
            //kid
            foreach (kid deks in dek)
            {
                deks.kiddraw(_spriteBatch);
            }
            //car
            foreach(Car mini_car in _car)
            {
                mini_car.Draw(_spriteBatch);
            }
            if (type_click == "sign_select")
            {
                _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                _spriteBatch.DrawString(uid_font, holding_text, new Vector2(105, 590), Color.Black);
                if (!none_area.Contains(_mousestate.X, _mousestate.Y) & !none_area2.Contains(_mousestate.X, _mousestate.Y) & _mousestate.X < _graphics.GraphicsDevice.Viewport.Width & _mousestate.Y < _graphics.GraphicsDevice.Viewport.Height & _mousestate.X > 0 & _mousestate.Y > 0)
                {
                    _spriteBatch.Draw(sign_texture, new Vector2(_mousestate.X - (sign_texture.Width / 2), _mousestate.Y - (sign_texture.Height / 2)), Color.White);
                    lastms = _mousestate;
                }
            }
            _spriteBatch.Draw(board_ui, new Vector2(0,642), Color.White);
            //bar
            if (!attacked)
            {
                _spriteBatch.Draw(barcolor, bar, Color.LimeGreen);

            }
            //feedback
            foreach(Ceffect mini_effect in hit)
            {
                mini_effect.Draw(_spriteBatch);
            }
            //ui
            //_spriteBatch.Draw(barcolor, none_area2, Color.White);
            if(type_click == "buybird")
            {
                _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                _spriteBatch.DrawString(uid_font, holding_text +" "+ holding_price, new Vector2(100, 590), Color.Black);
            }
            else if (type_click == "police")
            {
                _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                _spriteBatch.DrawString(uid_font, holding_text + " " + holding_price, new Vector2(100, 590), Color.Black);
            }
            else if (type_click == "sign")
            {
                _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                _spriteBatch.DrawString(uid_font, holding_text + " " + holding_price, new Vector2(100, 590), Color.Black);
            }
            else if (type_click == "car")
            {
                _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                _spriteBatch.DrawString(uid_font, holding_text + " " + holding_price, new Vector2(85, 590), Color.Black);
            }
            _spriteBatch.Draw(bar_grob, new Vector2(68, 666), Color.White);
            _spriteBatch.Draw(buy_bird, button_bird, Color.White);
            if(c_sign == 0)
            {
                _spriteBatch.Draw(buy_sign, button_sign, Color.White);
            }
            else
            {
                _spriteBatch.Draw(buy_sign, button_sign, Color.Gray);
                _spriteBatch.DrawString(cooldown_text, ""+(int)c_sign, new Vector2(button_sign.X+80,button_sign.Y+34), Color.White);
            }
            if (c_car == 0)
            {
                _spriteBatch.Draw(buy_car, button_car, Color.White);
            }
            else
            {
                _spriteBatch.Draw(buy_car, button_car, Color.Gray);
                _spriteBatch.DrawString(cooldown_text, "" + (int)c_car, new Vector2(button_car.X+80, button_car.Y+34), Color.White);
            }

            if (c_police == 0)
            {
                _spriteBatch.Draw(buy_police, button_police, Color.White);
            }
            else
            {
                _spriteBatch.Draw(buy_police, button_police, Color.Gray);
                _spriteBatch.DrawString(cooldown_text, "" + (int)c_police, new Vector2(button_police.X + 60, button_police.Y + 22), Color.White);
            }
            _spriteBatch.DrawString(uid_font, Convert.ToString(money), new Vector2(77, 746), Color.Black);
            _spriteBatch.DrawString(uid_font, Convert.ToString(bird.Count), new Vector2(77, 710), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}