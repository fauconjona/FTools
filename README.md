
# FTools

I have noticed some "inexperienced" people are asking how to create 3D text and marker, or try to do so, but without success. So, I decided to create this resource to help them, but also, to facilitate the experienced one. Instead of creating a new loop each time you need a 3D text or marker, with a lots of conditions inside to check the distance and a control pressed, you just need to call a function.

## features

 - 3D text
 - Marker, with action event
 - Area (Sphere, Circle, Box, Rectangle and Custom)
 - Pickups (Custom)

#### 3D Text
It's basically a text drawn in the wold, you can set the color, scale, position, font and max distance.

#### Marker
This one is more interesting, you can create a simple marker, but you can add it some features. Like a 3D text
and an action event, that can trigger an event or a callback.

#### Area
Really cool feature too, you can create an area with the shape you want (Sphere, Circle, Box, Rectangle or Custom) then you can trigger event or callback when the player enter or exit.

Custom shape is a poly with the points you want, but I don't recommend to use 1000 points.

#### Pickups
What is the point? There are already pickups in GTAV, yes but not like these. They are props sync other the network with action event that can trigger an event. So you can use the model you want and make them dynamic.

#### Action event
I talked about `action event`, but what is it exactly? It can be attached to a marker or a pickup, then when the player is on it, you can trigger an event or a callback. You can set it as Auto or as a Control Pressed, like "Press E to do".

*Callback doesn't work with pickups*

### Planned
I don't know, you can ask if you have some ideas.

## How to install
Like all resources, just need to add the folder inside your `resources` directory. Then, just add `start FTools` at the beginning of your `server.cfg`.

## How to use

You can use FTools with exports and map directive, but how to do that?

You can find the full documentation here

### Exports
All features are available in `exports.FTools` or `exports["FTools"]`

*Example:*
```lua
--Create a 3D text
exports.FTools:CreateText3D(
	"text_id", -- Unique identifier
	"Hello world", -- Text
	0, -- Font
	{ R = 255, G = 255, B = 255 }, -- RGB color 
	{ X = 1.0, Y = 1.0 }, -- 2D Scale
	{ X = -601.23, Y = 256.78, Z = 64.2 }, -- World position
	30.0 -- Max distance
)
```
### Map directive
Another way is to use map files. You know? Like `map.lua` that you can find inside `fivem-map-hipster`, there are some `spawnpoint`, it's a map file. 

*Example:*
```lua
add3DText "text_id" {
	Text = "Hello World",
	Font = 0,
	Color = { R = 255, G = 255, B = 255 },
	Scale = { X = 1.0, Y = 1.0 },
	Pos = { X = -601.23, Y = 256.78, Z = 64.2 },
	MaxDistance = 30.0
}
```

## Suggestions

I tried to make it more optimized as possible, but if you have any suggestion, you can tell me. 
