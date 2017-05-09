using System;
using Newtonsoft.Json;
using VRApplicationWF.Properties;

namespace VRApplicationWF
{
    internal class JSONMethods
    {
        private static string id;
        private static string routeUUID;

        public JSONMethods(string stringid)
        {
            id = stringid;
        }

        public static string PanelUuid { private get; set; }

        public static string BikeUuid { private get; set; }

        //Creates a tunnel, possible to add a password.
        public static string CreateTunnel(string sessionid, string passKey)
        {
            dynamic obj = new
            {
                id = "tunnel/create",
                data = new
                {
                    session = sessionid,
                    key = passKey
                }
            };

            return JsonConvert.SerializeObject(obj);
        }

        private static string Connection(dynamic packet)
        {
            dynamic obj = new
            {
                id = "tunnel/send",
                data = new
                {
                    dest = id,
                    data = packet
                }
            };
            return JsonConvert.SerializeObject(obj);
        }


        //gets the scene in a long string with all the information within that scene
        public string GetScene()
        {
            dynamic packet = new
            {
                id = "scene/get"
            };

            return Connection(packet);
        }

        public string ResetScene()
        {
            dynamic packet = new
            {
                id = "scene/reset",
                data = new
                {
                }
            };

            return Connection(packet);
        }

        //list of sessions available
        public static string SessionList()
        {
            dynamic obj = new
            {
                id = "session/list"
            };
            return JsonConvert.SerializeObject(obj);
        }

        //Add an object
        public string AddNode(string fileName, string nodeName, string bikeAnim = "", bool isAnimated = false,
            float scaling = 1.0f, float xPos = 0, float yPos = 0, float zPos = 0)
        {
            dynamic packet = new
            {
                id = "scene/node/add",
                data = new
                {
                    name = nodeName,
                    components = new
                    {
                        transform = new
                        {
                            position = new[] {xPos, yPos, zPos},
                            scale = scaling
                        },
                        model = new
                        {
                            file = fileName,
                            animated = isAnimated,
                            animation = bikeAnim
                        }
                    }
                }
            };

            return Connection(packet);
        }

        public static string GetNode(string nodeName)
        {
            dynamic packet = new
            {
                id = "scene/node/find",
                data = new
                {
                    name = nodeName
                }
            };

            return Connection(packet);
        }

        public string UpdateNode(string idInput)
        {
            dynamic packet = new
            {
                id = "scene/node/update",
                data = new
                {
                    id = idInput,
                    transform = new
                    {
                        position = new[] {30f, 70f, 0f},
                        scale = 1/0.02f,
                        rotation = new[] {0, 90, 0}
                    }
                }
            };
            return Connection(packet);
        }

        public static string UpdateNodeBike(double speedInput)
        {
            dynamic packet = new
            {
                id = "scene/node/update",
                data = new
                {
                    id = BikeUuid,
                    animation = new
                    {
                        name = "Armature|Fietsen",
                        speed = speedInput
                    }
                }
            };
            return Connection(packet);
        }

        //parents 2 nodes together, (only used to parent bike and camera)
        public string UpdateNodeParent(string idInput, string parentInput = "")
        {
            dynamic packet = new
            {
                id = "scene/node/update",
                data = new
                {
                    id = idInput,
                    parent = parentInput,
                    transform = new
                    {
                        scale = 1/0.02f
                    }
                }
            };

            return Connection(packet);
        }

        //add an object specific for terrain perposes
        public string AddNodeTerrain(string nodeName, float scaling = 1.0f, float xPos = 0f, float yPos = 0f,
            float zPos = 0f)
        {
            dynamic packet = new
            {
                id = "scene/node/add",
                data = new
                {
                    name = nodeName,
                    components = new
                    {
                        transform = new
                        {
                            position = new[] {-128f, yPos, -128f},
                            scale = scaling
                        },
                        terrain = new
                        {
                        }
                    }
                }
            };

            return Connection(packet);
        }

        public string AddNodeWater(string nodeName, float scaling = 1.0f, float xPos = 0f, float yPos = 0f,
            float zPos = 0f)
        {
            dynamic packet = new
            {
                id = "scene/node/add",
                data = new
                {
                    name = nodeName,
                    components = new
                    {
                        transform = new
                        {
                            position = new[] {0, 0.01, 0},
                            scale = scaling
                        },
                        water = new
                        {
                            size = new[] {20, 20},
                            resolution = 0.1f
                        }
                    }
                }
            };

            return Connection(packet);
        }


        //add a layer to add a texture to a layer as example an terrain
        public string AddLayer(string uuid, string diffuseimg, string normaltextureimg, int minHeight, int maxHeight)
        {
            dynamic packet = new
            {
                id = "scene/node/addlayer",
                data = new
                {
                    id = uuid,
                    diffuse = normaltextureimg,
                    normal = normaltextureimg,
                    minHeight = minHeight,
                    maxHeight = maxHeight,
                    fadeDist = 300
                }
            };

            return Connection(packet);
        }

        //Remove object
        public string RemoveObject(string uuid)
        {
            dynamic packet = new
            {
                id = "scene/node/delete",
                data = new
                {
                    id = uuid
                }
            };

            return Connection(packet);
        }

        //change the time, value between 00 and 24
        public string SetTime()
        {
            dynamic packet = new
            {
                id = "scene/skybox/settime",
                data = new
                {
                    time = 12
                }
            };

            return Connection(packet);
        }

        //Update the terrain.
        public string TerrainUpdate()
        {
            dynamic packet = new
            {
                id = "scene/terrain/update",
                data = new
                {
                }
            };

            return Connection(packet);
        }

        //Delete the terrain.
        public string TerrainDelete()
        {
            dynamic packet = new
            {
                id = "scene/terrain/delete",
                data = new
                {
                }
            };

            return Connection(packet);
        }

        //Create a terrain with a heightmap of 0 all over.
        public string AddFlatTerrain(float length = 512, float width = 512)
        {
            dynamic packet = new
            {
                id = "scene/terrain/add",
                data = new
                {
                    size = new[] {length, width},
                    heights = HeightMapToArray()
                }
            };

            return Connection(packet);
        }

        public string AddRoad(string uuid)
        {
            routeUUID = uuid;
            dynamic packet = new
            {
                id = "scene/road/add",
                data = new
                {
                    route = routeUUID
                }
            };

            return Connection(packet);
        }

        public string AddRoute()
        {
            var nodesI = new dynamic[4];
            nodesI[0] = AddRouteGetPosition(-50, 0, 50, -45, 0, 55);
            nodesI[1] = AddRouteGetPosition(-50, 0, -50, -45, 0, -45);

            nodesI[2] = AddRouteGetPosition(50, 0, -50, 45, 0, -45);
            nodesI[3] = AddRouteGetPosition(50, 0, 50, -45, 0, -45);


            dynamic packet = new
            {
                id = "route/add",
                data = new
                {
                    nodes = nodesI
                }
            };

            return Connection(packet);
        }

        public string AddSkyBox()
        {
            dynamic packet = new
            {
                id = "scene/skybox/update",
                data = new
                {
                    type = "static",
                    files = new
                    {
                        xpos = "data/NetworkEngine/textures/SkyBoxes/interstellar/interstellar_rt.png",
                        xneg = "data/NetworkEngine/textures/SkyBoxes/interstellar/interstellar_lf.png",
                        ypos = "data/NetworkEngine/textures/SkyBoxes/interstellar/interstellar_up.png",
                        yneg = "data/NetworkEngine/textures/SkyBoxes/interstellar/interstellar_dn.png",
                        zpos = "data/NetworkEngine/textures/SkyBoxes/interstellar/interstellar_bk.png",
                        zneg = "data/NetworkEngine/textures/SkyBoxes/interstellar/interstellar_ft.png"
                    }
                }
            };
            return Connection(packet);
        }

        //follow route with the uuid of one node (all other objects are parented to this)
        public string FollowRoute(string nodeUuid)
        {
            dynamic packet = new
            {
                id = "route/follow",
                data = new
                {
                    route = routeUUID,
                    node = nodeUuid,
                    speed = 5.0,
                    offset = 0.0,
                    rotate = "XZ",
                    followHeight = true,
                    rotateOffset = new[] {0, 0, 0},
                    positionOffset = new[] {0, 0, 0}
                }
            };
            return Connection(packet);
        }

        //doesn't work
        //makes the debug route line in scene visible or unvisible
        public string ShowRoute(bool showRoute)
        {
            dynamic packet = new
            {
                id = "route/show",
                data = new
                {
                    show = showRoute
                }
            };
            return Connection(packet);
        }

        public static string SetSpeed(double speedValue)
        {
            dynamic packet = new
            {
                id = "route/follow/speed",
                data = new
                {
                    node = BikeUuid,
                    speed = speedValue
                }
            };
            return Connection(packet);
        }

        public string AddNodePanel(string nodeName, string parentName, float scaling = 1.0f, int xPos = 0, int yPos = 0,
            int zPos = 0)
        {
            dynamic packet = new
            {
                id = "scene/node/add",
                data = new
                {
                    name = nodeName,
                    parent = parentName,
                    components = new
                    {
                        transform = new
                        {
                            position = new[] {-0.02f, 1.0f, -1.3f},
                            scale = scaling,
                            rotation = new[] {-49f, 0f, 0f}
                        },
                        panel = new
                        {
                            size = new[] {0.97f, 0.69f},
                            resolution = new[] {512, 512},
                            background = new[] {1, 1, 1, 1}
                        }
                    }
                }
            };

            return Connection(packet);
        }

        //creates the buffer for the panel, has to be called when everything is finished drawing on the panel
        public static string swapPanel()
        {
            dynamic packet = new
            {
                id = "scene/panel/swap",
                data = new
                {
                    id = PanelUuid
                }
            };
            return Connection(packet);
        }

        public static string ClearPanel()
        {
            dynamic packet = new
            {
                id = "scene/panel/clear",
                data = new
                {
                    id = PanelUuid
                }
            };
            return Connection(packet);
        }

        public static string AddPanelText(string panelText, float z, float x)
        {
            dynamic packet = new
            {
                id = "scene/panel/drawtext",
                data = new
                {
                    id = PanelUuid,
                    text = panelText,
                    position = new[] {x, z},
                    size = 32.0f,
                    color = new[] {1, 1, 1, 1},
                    font = "Segoeui"
                }
            };
            return Connection(packet);
        }

        public string SetPanelClearColor()
        {
            dynamic packet = new
            {
                id = "scene/panel/setclearcolor",
                data = new
                {
                    id = PanelUuid,
                    color = new[] {0, 0, 0, 1}
                }
            };
            return Connection(packet);
        }

        public static string Play()
        {
            dynamic packet = new
            {
                id = "play",
                data = new
                {
                }
            };
            return Connection(packet);
        }

        public static string Pause()
        {
            dynamic packet = new
            {
                id = "pause",
                data = new
                {
                }
            };
            return Connection(packet);
        }  

        //helpmethods which are returning for specific purposes (dynamics, arrays...)
        private float[] HeightMapToArray()
        {
            var heightArray = new float[512*512];

            var img = Resources.heightmap1;


            for (var j = 0; j < img.Height; j++)
                for (var i = 0; i < img.Width; i++)
                {
                    var pixel = img.GetPixel(j, i);

                    var bytes = pixel.B;

                    heightArray[i + j*512] = Convert.ToInt32(bytes)/4.0f;
                }

            Console.WriteLine(
                heightArray.Length);

            return heightArray;
        }

        public dynamic AddRouteGetPosition(float x, float y, float z, float dirx, float diry, float dirz)
        {
            dynamic position = new
            {
                pos = new[] {x, y, z},
                dir = new[] {dirx, diry, dirz}
            };
            return position;
        }
    }
}