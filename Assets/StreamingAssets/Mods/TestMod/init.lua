print("Инициализация мода...")

--local Spawner = require("spawner")

Events:On("GameStart", function()
    print("Игра началась!")
    UI:ShowButton("Призвать дерево", function()
        local id = World:Spawn("Tree", { x = 6, y = 0, z = -3 })
        if id then
            print("Посажено дерево с ID: " .. tostring(id))
        end
    end)
end)