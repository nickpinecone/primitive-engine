local shift = Vector2(0, 1)
local pad = root:GetEntity("pad")

function update()
    position = position + shift

    if this:Collide(pad) then
        shift = Vector2(0, -1)
    elseif position.Y <= 0 then
        shift = Vector2(0, 1)
    end
end
