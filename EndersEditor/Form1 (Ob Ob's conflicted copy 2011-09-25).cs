using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Enders2;
using EndersEditorCommon;

namespace EndersEditor
{
    using Image = System.Drawing.Image;
    using Microsoft.Xna.Framework.Content;

    public partial class Form1 : Form
    {
        const int MaxFillCells = 1000;

        string[] imageExtensions = new string[]
        {
            ".jpg", ".png", ".tga",
        };

        int maxWidth = 0, maxHeight = 0;

        SpriteBatch spriteBatch;

        Texture2D unPassable;
        Texture2D slowDown;
        Texture2D doorStop;
        Texture2D tileTexture; 
        Texture2D PlayerPoint;
        Texture2D EnemyPoint;
        Texture2D MiscPoint;
        SpriteFont spriteFont;

        TileLayer currentLayer;
        CollisionLayer currentCollisionLayer;
        private static WinformsContentManager contentManager;

        int cellX, cellY, collideCellX, collideCellY;

        bool MouseDown = false;
        bool coolBool = false;

        Camera camera = new Camera();

        TileMap tileMap = new TileMap();

        int FillCounter = MaxFillCells;

        Dictionary<string, TileLayer> layerDict = new Dictionary<string, TileLayer>();
        Dictionary<string, CollisionLayer> collDict = new Dictionary<string, CollisionLayer>();

        Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
        Dictionary<string, Image> previewDict = new Dictionary<string, Image>();

        Dictionary<string, Texture2D> collisionPics = new Dictionary<string, Texture2D>();

        ManageEntities form = new ManageEntities();
        Associate_Number numForm = new Associate_Number();

        ReadPoint point = new ReadPoint();

        string Spawntype;
        string tileNumber;

        int tileNumberKind;
        bool isDrawmNum;

        public GraphicsDevice GraphicsDevice
        {
            get { return tileDisplay1.GraphicsDevice; }

        }

        /// <summary>
        /// Public content manager
        /// </summary>
        public static WinformsContentManager ContentManager
        {
            get { return contentManager; }
        }

        public Form1()
        {
            InitializeComponent();

            tileDisplay1.OnInitialize += new EventHandler(tileDisplay1_OnInitialize);
            tileDisplay1.OnDraw += new EventHandler(tileDisplay1_OnDraw);

            Application.Idle += delegate { tileDisplay1.Invalidate(); };

            saveFileDialog1.Filter = "Map File|*.map";

            Mouse.WindowHandle = tileDisplay1.Handle;
        }

        void tileDisplay1_OnInitialize(object sender, EventArgs e)
        { 

            spriteBatch = new SpriteBatch(GraphicsDevice);
            string buildError;
            string currentDirectory = FileHelper.GetAssemblyDirectory();

            ContentBuilder builder = new ContentBuilder();

            // create content manager
            ContentManager manager = new ContentManager(tileDisplay1.Services,
                                    builder.OutputDirectory);

            contentManager = new WinformsContentManager(builder, manager);

           
            spriteFont = contentManager.LoadContent<SpriteFont>
                (currentDirectory + "\\Content\\hudFont.spritefont", null,
                "FontDescriptionProcessor", out buildError, false, false);

            using (Stream stream = File.OpenRead("Content/tile.png"))
            {
                tileTexture = Texture2D.FromStream(GraphicsDevice, stream);
            }

            using (Stream stream = File.OpenRead("Content/DoorStop.png"))
            {
                doorStop = Texture2D.FromStream(GraphicsDevice, stream);
            }

            using (Stream stream1 = File.OpenRead("Content/unPassable.png"))
            {
                unPassable = Texture2D.FromStream(GraphicsDevice, stream1);
            }

            using (Stream stream2 = File.OpenRead("Content/slowDown.png"))
            {
                slowDown = Texture2D.FromStream(GraphicsDevice, stream2);
            }

            using (Stream stream3 = File.OpenRead("Content/PlayerPoint.png"))
            {
                PlayerPoint = Texture2D.FromStream(GraphicsDevice, stream3);
            }

            using (Stream stream4 = File.OpenRead("Content/EnemyPoint.png"))
            {
                EnemyPoint = Texture2D.FromStream(GraphicsDevice, stream4);
            }

            using (Stream stream5 = File.OpenRead("Content/MiscPoint.png"))
            {
                MiscPoint = Texture2D.FromStream(GraphicsDevice, stream5);
            }

            collisionPics.Add("Unpassable", unPassable);
            collisionPics.Add("Platform", slowDown);
            collisionPics.Add("DoorStop", doorStop);

            collisionTiles.Items.Add("Erase");

            foreach (var picName in collisionPics)
                collisionTiles.Items.Add(picName.Key);

            collisionTiles.Items.Add("SpawnDraw");

            point.ReadFile(point.PlayerFile);
            point.ReadFile(point.EnemyFile);
            point.ReadFile(point.MiscFile);

            AssociateBox.Items.Add("Do Nothing");
            AssociateBox.Items.Add("Associate");
            AssociateBox.Items.Add("Unassociate");

            AssociateBox.SelectedIndex = 0;
        }

        void tileDisplay1_OnDraw(object sender, EventArgs e)
        {
            Logic();
            Render();
        }

        public void FillCell (int x, int y, int desiredIndex)
        {
            int oldIndex = currentLayer.GetCellIndex(x, y);

            if (desiredIndex == oldIndex || FillCounter == 0)
                return;

            FillCounter--;

            currentLayer.SetCellIndex(x, y, desiredIndex);

            if (x > 0 && currentLayer.GetCellIndex(x - 1, y) == oldIndex)
                FillCell(x - 1, y, desiredIndex);
            if (x < currentLayer.Width - 1 && currentLayer.GetCellIndex(x + 1, y) == oldIndex)
                FillCell(x + 1, y, desiredIndex);
            if (y > 0 && currentLayer.GetCellIndex(x, y - 1) == oldIndex)
                FillCell(x, y - 1, desiredIndex);
            if (y < currentLayer.Height - 1 && currentLayer.GetCellIndex(x, y + 1) == oldIndex)
                FillCell(x, y + 1, desiredIndex);
         }

        private void tileDisplay1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDown = true;
        }

        private void tileDisplay1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDown = false;
        }

        private void Logic()
        {
            camera.Position.X = hScrollBar1.Value * Engine.TileWidth;
            camera.Position.Y = vScrollBar1.Value * Engine.TileHeight;

            int mx = Mouse.GetState().X;
            int my = Mouse.GetState().Y;

            if (currentCollisionLayer != null)
            {
                if (mx >= 0 && mx < tileDisplay1.Width &&
                    my >= 0 && my < tileDisplay1.Height)
                {
                    collideCellX = mx / Engine.TileWidth;
                    collideCellY = my / Engine.TileHeight;

                    collideCellX += hScrollBar1.Value;
                    collideCellY += vScrollBar1.Value;

                    collideCellX = (int)MathHelper.Clamp(collideCellX, 0, currentCollisionLayer.Width-1);
                    collideCellY = (int)MathHelper.Clamp(collideCellY, 0, currentCollisionLayer.Height-1);
                }
            }

            if (currentLayer != null)
            {

                if (mx >= 0 && mx < tileDisplay1.Width &&
                    my >= 0 && my < tileDisplay1.Height)
                {

                    cellX = mx / Engine.TileWidth;
                    cellY = my / Engine.TileHeight;

                    cellX += hScrollBar1.Value;
                    cellY += vScrollBar1.Value;

                    cellX = (int)MathHelper.Clamp(cellX, 0, currentLayer.Width - 1);
                    cellY = (int)MathHelper.Clamp(cellY, 0, currentLayer.Height - 1);

                    if (MouseDown && coolBool == false)
                    {
                        if (drawRadioButton.Checked && textureListBox != null)
                        {
                            Texture2D texture = textureDict[textureListBox.SelectedItem as string];

                            int index = currentLayer.isUsingTexture(texture);

                            if (index == -1)
                            {
                                currentLayer.AddTexture(texture);
                                index = currentLayer.isUsingTexture(texture);
                            }

                            if (fillCheckBox.Checked)
                            {
                                FillCounter = 500;
                                FillCell(cellX, cellY, index);
                            }
                            else
                                currentLayer.SetCellIndex(cellX, cellY, index);
                        }
                        else if (eraseRadioButton.Checked)
                        {
                            if (fillCheckBox.Checked)
                            {
                                FillCounter = 500;
                                FillCell(cellX, cellY, -1);
                            }
                            else
                                currentLayer.SetCellIndex(cellX, cellY, -1);
                        }
                    }
                }
                else
                {
                    cellX = cellY = -1;
                }
            }

            if (MouseDown && coolBool == true)
            {
                tileNumber = currentCollisionLayer.GetCellIndex(collideCellX, collideCellY);

                if (drawRadioButton.Checked && textureListBox != null)
                {
                    int colIndex = collisionTiles.SelectedIndex;

                    if (colIndex == 0 && collisionTiles.SelectedItem != null && AssociateBox.SelectedIndex == 0)
                        currentCollisionLayer.SetCellIndex(collideCellX, collideCellY, colIndex.ToString());

                    spriteBatch.Begin();

                    if (colIndex == 1 && AssociateBox.SelectedIndex == 0)
                    {
                        currentCollisionLayer.SetCellIndex(collideCellX, collideCellY, colIndex.ToString());
                    }

                    if (colIndex == 2 && AssociateBox.SelectedIndex == 0)
                    {
                        currentCollisionLayer.SetCellIndex(collideCellX, collideCellY, colIndex.ToString());
                    }

                    if (colIndex == 3 && AssociateBox.SelectedIndex == 0)
                    {
                        currentCollisionLayer.SetCellIndex(collideCellX, collideCellY, colIndex.ToString());
                    }

                    if (colIndex == 4 && AssociateBox.SelectedIndex == 0)
                    {
                        currentCollisionLayer.SetCellIndex(collideCellX, collideCellY, form.spawnNumber.ToString());
                    }

                    if (AssociateBox.SelectedIndex == 1)
                    {
                        string TileName = tileNumber + "#" + numForm.actualNumber.Text;
                        int count = TileName.Count(f => f == '#');

                        if (count <= 1 && int.Parse(tileNumber) > 3)
                        {
                            currentCollisionLayer.SetCellIndex(collideCellX, collideCellY, TileName);
                        }
                        else
                        {
                            TileName = null;
                        }
                    }

                    else if (AssociateBox.SelectedIndex == 2)
                    {
                        int tileNumberType;
                        bool isNum = int.TryParse(tileNumber, out tileNumberType);

                        if (!isNum)
                        {
                            string[] numbers = tileNumber.Split('#');
                            currentCollisionLayer.SetCellIndex(collideCellX, collideCellY, numbers[0]);
                        }
                        else
                        {
                            currentCollisionLayer.SetCellIndex(collideCellX, collideCellY, tileNumber);
                        }
                    }
                    spriteBatch.End();
                }
            }
        }
            
        private void Render()
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (TileLayer layer in tileMap.Layers)
            {
                layer.Draw(spriteBatch, camera);

                spriteBatch.Begin();
                for (int y = 0; y < layer.Height; y++)
                {
                    if (showGridToolStripMenuItem.Checked && layer == currentLayer)
                    {

                        for (int x = 0; x < layer.Width; x++)
                        {
                            if (layer.GetCellIndex(x, y) == -1)
                            {
                                spriteBatch.Draw(
                                tileTexture,
                                new Rectangle(x * Engine.TileWidth - (int)camera.Position.X,
                                              y * Engine.TileHeight - (int)camera.Position.Y,
                                              Engine.TileWidth,
                                              Engine.TileHeight),
                                Color.White);
                            }
                        }
                    }
                }

                spriteBatch.End();

                if (!showAllLayersToolStripMenuItem.Checked && layer == currentLayer)
                    break;
            }
             if (coolBool == true && currentCollisionLayer != null)
            {
                spriteBatch.Begin();

                for (int y = 0; y < currentCollisionLayer.Height; y++)
                {
                    for (int x = 0; x < currentCollisionLayer.Width; x++)
                    {
                        if (currentCollisionLayer.GetCellIndex(x, y) == "1")
                        {
                            DrawCollideTile(x, y, unPassable);
                        }
                        else if (currentCollisionLayer.GetCellIndex(x, y) == "2")
                        {
                            DrawCollideTile(x, y, slowDown);
                        }
                        else if (currentCollisionLayer.GetCellIndex(x, y) == "3")
                        {
                            DrawCollideTile(x, y, doorStop);
                        }
                        else if (currentCollisionLayer.GetCellIndex(x, y) == "0")
                        {
                            if (showGridToolStripMenuItem.Checked && coolBool == true)
                                DrawCollideTile(x, y, tileTexture);
                        }
                        else
                        {
                            string tileNo = currentCollisionLayer.GetCellIndex(x, y);
                            isDrawmNum = int.TryParse(tileNo, out tileNumberKind);

                            if (isDrawmNum)
                            {

                                foreach (KeyValuePair<string, string> value in point.Players)
                                {
                                    if (value.Key == currentCollisionLayer.GetCellIndex(x, y))
                                    {
                                        Spawntype = "Player";
                                        DrawSpawnTile(x, y, Spawntype, value.Value, null);
                                    }
                                }

                                foreach (KeyValuePair<string, string> value in point.Enemies)
                                {
                                    if (value.Key == currentCollisionLayer.GetCellIndex(x, y))
                                    {
                                        Spawntype = "Enemy";
                                        DrawSpawnTile(x, y, Spawntype, value.Value, null);
                                    }
                                }
                                foreach (KeyValuePair<string, string> value in point.Miscellaneous)
                                {
                                    if (value.Key == currentCollisionLayer.GetCellIndex(x, y))
                                    {
                                        Spawntype = "Miscellaneous";
                                        DrawSpawnTile(x, y, Spawntype, value.Value, null);
                                    }
                                }
                            }
                            else if (!isDrawmNum)
                            {
                                string[] tileNoComb = tileNo.Split('#');

                                foreach (KeyValuePair<string, string> value in point.Players)
                                {
                                    if (value.Key == tileNoComb[0])
                                    {
                                        Spawntype = "Player";
                                        DrawSpawnTile(x, y, Spawntype, value.Value,tileNoComb[1]);
                                    }
                                }

                                foreach (KeyValuePair<string, string> value in point.Enemies)
                                {
                                    if (value.Key == tileNoComb[0])
                                    {
                                        Spawntype = "Enemy";
                                        DrawSpawnTile(x, y, Spawntype, value.Value,tileNoComb[1]);
                                    }
                                }
                                foreach (KeyValuePair<string, string> value in point.Miscellaneous)
                                {
                                    if (value.Key == tileNoComb[0])
                                    {
                                        Spawntype = "Miscellaneous";
                                        DrawSpawnTile(x, y, Spawntype, value.Value, tileNoComb[1]);
                                    }
                                }
                            }
                        }
                    }
                }
                spriteBatch.End();
            }

            if (currentLayer != null && coolBool == false)
            {
                if (cellX != -1 && cellY != -1)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(
                            tileTexture,
                            new Rectangle(cellX * Engine.TileWidth - (int)camera.Position.X,
                                          cellY * Engine.TileHeight - (int)camera.Position.Y,
                                          Engine.TileWidth,
                                          Engine.TileHeight),
                            Color.Red);
                    spriteBatch.End();
                }
            }

            if (currentCollisionLayer != null && coolBool == true)
            {
                if (collideCellX != 0 && collideCellY != 0)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(
                            tileTexture,
                            new Rectangle(collideCellX * Engine.TileWidth - (int)camera.Position.X,
                                          collideCellY * Engine.TileHeight - (int)camera.Position.Y,
                                          Engine.TileWidth,
                                          Engine.TileHeight),
                            Color.Blue);
                    spriteBatch.End();
                }
            }
        }

        private void newTileMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void DrawCollideTile(int x, int y, Texture2D texture)
        {
            spriteBatch.Draw(
                            texture,
                            new Rectangle(x * Engine.TileWidth - (int)camera.Position.X,
                                          y * Engine.TileHeight - (int)camera.Position.Y,
                                          Engine.TileWidth,
                                          Engine.TileHeight),
                            Color.White);
        }

        private void DrawSpawnTile(int x, int y, string type, string name, string associate)
        {
            if (type == "Player")
            {
                spriteBatch.Draw(
                                PlayerPoint,
                                new Rectangle(x * Engine.TileWidth - (int)camera.Position.X,
                                              y * Engine.TileHeight - (int)camera.Position.Y,
                                              Engine.TileWidth,
                                              Engine.TileHeight),
                               Color.White);

                spriteBatch.DrawString(spriteFont, name, new Vector2(10+x * Engine.TileWidth - (int)camera.Position.X, y * Engine.TileHeight - (int)camera.Position.Y), Color.Black);

                if(associate != null)
                spriteBatch.DrawString(spriteFont, associate, new Vector2(25 + x * Engine.TileWidth - +(int)camera.Position.X, 20 + y * Engine.TileHeight - (int)camera.Position.Y), Color.Red); 
            }

            if (type == "Enemies")
            {
                spriteBatch.Draw(
                                EnemyPoint,
                                new Rectangle(x * Engine.TileWidth - (int)camera.Position.X,
                                              y * Engine.TileHeight - (int)camera.Position.Y,
                                              Engine.TileWidth,
                                              Engine.TileHeight),
                                Color.White);
                spriteBatch.DrawString(spriteFont, name, new Vector2(10+x * Engine.TileWidth - (int)camera.Position.X, y * Engine.TileHeight - (int)camera.Position.Y), Color.Black);
                if (associate != null)
                    spriteBatch.DrawString(spriteFont, associate, new Vector2(25 + x * Engine.TileWidth - +(int)camera.Position.X, 20 + y * Engine.TileHeight - (int)camera.Position.Y), Color.Red); 
            }

            if (type == "Miscellaneous")
            {
                spriteBatch.Draw(
                                MiscPoint,
                                new Rectangle(x * Engine.TileWidth - (int)camera.Position.X,
                                              y * Engine.TileHeight - (int)camera.Position.Y,
                                              Engine.TileWidth,
                                              Engine.TileHeight),
                                Color.White);
                spriteBatch.DrawString(spriteFont, name, new Vector2(10+x * Engine.TileWidth - (int)camera.Position.X, y * Engine.TileHeight - (int)camera.Position.Y), Color.Black);
                if (associate != null)
                    spriteBatch.DrawString(spriteFont, associate, new Vector2(25 + x * Engine.TileWidth - +(int)camera.Position.X, 20 + y * Engine.TileHeight - (int)camera.Position.Y), Color.Red); 
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Map File|*.map";
            openFileDialog1.Multiselect = false;

                //tileMap = null;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                layerDict.Clear();
                collDict.Clear();
                textureDict.Clear();
                previewDict.Clear();


                CollLayerListBox.Items.Clear();
                TilelayerListBox.Items.Clear();
                textureListBox.Items.Clear();

                tileMap.tile.Clear();
                tileMap.coll.Clear();

                tileMap.Layers.Clear();
                tileMap.colLayers.Clear();

                Logic();
                Render();

                currentLayer = null;
                currentCollisionLayer = null;


                string filename = openFileDialog1.FileName;

                string[] textureNames;

                 tileMap.OpenTileFile(filename);

                foreach (string tile in tileMap.tile) // Loop through List with foreach
                {

                    TileLayer tileLayer = TileLayer.FromFile(contentPathTextBox.Text + "/" + tile, out textureNames);
                    
                    layerDict.Add(Path.GetFileName(contentPathTextBox.Text + "/" + tile), tileLayer);
                    
                    tileMap.Layers.Add(tileLayer);
                    
                    TilelayerListBox.Items.Add(Path.GetFileName(contentPathTextBox.Text + "/" + tile));

                    foreach (string textureName in textureNames)
                    {
                        if (textureDict.ContainsKey(textureName))
                        {
                            tileLayer.AddTexture(textureDict[textureName]);
                            continue;
                        }

                        string fullPath = contentPathTextBox.Text + "/" + textureName;

                        foreach (string ext in imageExtensions)
                        {
                            if (File.Exists(fullPath + ext))
                            {
                                fullPath += ext;
                                break;
                            }
                        }

                        Texture2D tex;

                        using (Stream stream = File.OpenRead(fullPath))
                        {
                            tex = Texture2D.FromStream(GraphicsDevice, stream);
                        }

                        Image image = Image.FromFile(fullPath);
                        textureDict.Add(textureName, tex);
                        previewDict.Add(textureName, image);

                        textureListBox.Items.Add(textureName);
                        tileLayer.AddTexture(tex);
                    }
                }

                foreach (string coll in tileMap.coll) // Loop through List with foreach
                {
                    CollisionLayer collLayer = CollisionLayer.FromFile(contentPathTextBox.Text + "/" + coll);
                    collDict.Add(Path.GetFileName(contentPathTextBox.Text + "/" + coll), collLayer);

                    tileMap.colLayers.Add(collLayer);
                    CollLayerListBox.Items.Add(Path.GetFileName(contentPathTextBox.Text + "/" + coll));
                }
               
                AdjustScrollBars();
            }
        }

        private void AdjustScrollBars()
        {
            if (tileMap.GetWidthInPixels() > tileDisplay1.Width)
            {
                maxWidth = (int)Math.Max(tileMap.GetWidth(), maxWidth);

                hScrollBar1.Visible = true;
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = maxWidth;
            }
            else
            {
                maxWidth = 0;
                hScrollBar1.Visible = false;
            }

            if (tileMap.GetHeightInPixels() > tileDisplay1.Height)
            {
                maxHeight = (int)Math.Max(tileMap.GetHeight(), maxHeight);

                vScrollBar1.Visible = true;
                vScrollBar1.Minimum = 0;
                vScrollBar1.Maximum = maxHeight;
            }
            else
            {
                maxHeight = 0;
                vScrollBar1.Visible = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> tileNameList = new List<string>();
            List<string> collNameList = new List<string>();

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                foreach (var tileName in layerDict)
                    tileNameList.Add(tileName.Key);

                foreach (var colName in collDict)
                    collNameList.Add(colName.Key);


                foreach (var layer in layerDict)
                {
                    TileLayer tileLayer = layer.Value;

                    Dictionary<int, string> utilizedTextures = new Dictionary<int, string>();

                    foreach (string textureName in textureListBox.Items)
                    {
                        int index = tileLayer.isUsingTexture(textureDict[textureName]);

                        if (index != -1)
                        {
                            utilizedTextures.Add(index, textureName);
                        }
                    }
                    List<string> utilizedTextureList = new List<string>();

                    for (int i = 0; i < utilizedTextures.Count; i++)
                        utilizedTextureList.Add(utilizedTextures[i]);

                    tileMap.Save(saveFileDialog1.FileName, collNameList.ToArray(), tileNameList.ToArray());
                    tileLayer.Save(contentPathTextBox.Text + "\\Layers\\" + layer.Key, utilizedTextureList.ToArray());
                }
                foreach (var colLayer in collDict)
                {
                    CollisionLayer collLayer = colLayer.Value;
                    collLayer.Save(contentPathTextBox.Text + "\\Layers\\" + colLayer.Key);
                }
            } 
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textureListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textureListBox.SelectedItem != null)
            {
                texturePreviewBox.Image = previewDict[textureListBox.SelectedItem as string];
            }
        }

        private void layerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (TilelayerListBox.SelectedItem != null)
            {
                    currentLayer = layerDict[TilelayerListBox.SelectedItem.ToString()];
                    alphaSlider.Value = (int)(currentLayer.Alpha * 100);
                    coolBool = false;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (currentLayer != null)
                currentLayer.Alpha = (float)alphaSlider.Value / 100f;
        }

        private void AddLayerButton_Click(object sender, EventArgs e)
        {
            newLayerForm form = new newLayerForm();

            form.ShowDialog();

           if (form.OKpressed)
            {
                TileLayer tileLayer = new TileLayer(
                    int.Parse(form.width.Text),
                    int.Parse(form.height.Text));

                layerDict.Add(form.name.Text + ".layer", tileLayer);
                tileMap.Layers.Add(tileLayer);
                TilelayerListBox.Items.Add(form.name.Text + ".layer");

                AdjustScrollBars();
            }
        }

        private void removeLayerButton_Click(object sender, EventArgs e)
        {
             string filename = TilelayerListBox.SelectedItem as string;

             if (currentLayer != null)
             {
                 tileMap.Layers.Remove(currentLayer);
                 layerDict.Remove(filename);
                 TilelayerListBox.Items.Remove(TilelayerListBox.SelectedItem);

                 currentLayer = null;

                 AdjustScrollBars();
             }
        }

        private void addTextureButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG Image|*.jpg|PNG Image|*.png|TGA Image|*.tga";
            openFileDialog1.Multiselect = true;
            openFileDialog1.InitialDirectory = contentPathTextBox.Text;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string fullFilename in openFileDialog1.FileNames)
                {
                    string filename = fullFilename;

                    Texture2D texture;

                    using (Stream stream = File.OpenRead(filename))
                    {
                        texture = Texture2D.FromStream(GraphicsDevice, stream);
                    }

                    Image image = Image.FromFile(filename);

                    filename = filename.Replace(contentPathTextBox.Text + "\\", "");
                    //filename = filename.Remove(filename.LastIndexOf("."));

                    textureListBox.Items.Add(filename);
                    textureDict.Add(filename, texture);
                    previewDict.Add(filename, image);
                }
            }
        }

        private void removeTextureButton_Click(object sender, EventArgs e)
        {
            if (textureListBox.SelectedItem != null)
            {
                string textureName = textureListBox.SelectedItem as string;

                foreach (TileLayer layer in tileMap.Layers)
                    if (layer.isUsingTexture(textureDict[textureName]) != -1)
                        layer.RemoveTexture(textureDict[textureName]);

                textureDict.Remove(textureName);
                previewDict.Remove(textureName);
                textureListBox.Items.Remove(textureListBox.SelectedItem);

                texturePreviewBox.Image = null;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                contentPathTextBox.Text = folderBrowserDialog1.SelectedPath;
            else
                Close();
        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form.ShowDialog();
        }

        private void AddCollLayerButton_Click(object sender, EventArgs e)
        {
            newCollisionLayerForm form2 = new newCollisionLayerForm();

            form2.ShowDialog();

            if (form2.OKColpressed)
            {
                CollisionLayer collisionLayer = new CollisionLayer(
                  int.Parse(form2.collWidth.Text),
                  int.Parse(form2.collHeight.Text));

                  collDict.Add(form2.collName.Text + ".collayer", collisionLayer);
                  tileMap.colLayers.Add(collisionLayer);
                  CollLayerListBox.Items.Add(form2.collName.Text + ".collayer");
               
                AdjustScrollBars();
            }
        }

        private void removeCollLayerButton_Click(object sender, EventArgs e)
        {
            string filename = CollLayerListBox.SelectedItem as string;

            if (CollLayerListBox.SelectedItem != null)
            {
                tileMap.colLayers.Remove(currentCollisionLayer);
                collDict.Remove(filename);
                CollLayerListBox.Items.Remove(CollLayerListBox.SelectedItem);

                currentCollisionLayer = null;
            }
        }

        private void CollLayerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CollLayerListBox.SelectedItem != null)
            {
                currentCollisionLayer = collDict[CollLayerListBox.SelectedItem.ToString()];
                coolBool = true;
            }

        }

        private void withNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            numForm.ShowDialog();
        }
    }
}