local Spawner = {}

function Spawner.Tree(position)
    local id = World:Spawn("Tree", position)
    return id
end

return Spawner