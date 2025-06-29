print("Загружаем мод...")

local Spawner = require("spawner")

Events:On("GameStart", function()
    print("Игра началась!")

    UI:ShowButton("Призвать дерево", function()
        local position = { x = 6, y = 0, z = -3 }
        local id = Spawner.Tree(position)
        if id ~= nil then
            print("Посажено дерево с ID: " .. tostring(id))
        end
    end)
end)