position.X = screen.Width / 2 - size.X / 2
position.Y = screen.Height - size.Y

local dx = 120

function update(delta)
    if keyboard:IsKeyDown(Keys.D) then
        position.X = position.X + dx * delta
    elseif keyboard:IsKeyDown(Keys.A) then
        position.X = position.X - dx * delta
    end

    if position.X < 0 then
        position.X = 0
    elseif position.X > screen.Width - size.X then
        position.X = screen.Width - size.X
    end
end
