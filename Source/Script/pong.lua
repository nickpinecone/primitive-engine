position.X = screen.Width / 4 - size.X / 2

local dy = 90
local dx = 60
local velocity = Vector2(dx, dy)
local pad = root:GetEntity("pad")

function update(delta)
    position = position + velocity * delta

    if this:Collide(pad) or position.Y > screen.Height - size.Y then
        velocity = Vector2(velocity.X, -dy)
    elseif position.Y < 0 then
        velocity = Vector2(velocity.X, dy)
    end

    if position.X < 0 or position.X > screen.Width - size.X then
        velocity = Vector2(-velocity.X, velocity.Y)
    end
end
