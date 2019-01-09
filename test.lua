AddEventHandler('onClientResourceStart', function (resource)
    if resource == "FTools" then
        print('started wesh')
        exports['FTools']:addMarkerEvent(1, { X = -976.93, Y = -2996.31, Z = 12.95 }, { X = 3.0, Y = 3.0, Z = 1.0 }, { R = 255, G = 0, B = 0 }, "Something", 0, { R = 255, G = 255, B = 255 }, { X = 0.8, Y = 0.8 }, { X = -976.93, Y = -2996.31, Z = 14.45 }, 40.0, 0, 0, "", function()
            print('pressed')
        end)
        --exports['FTools']:addMarkerEvent(-976.93, -2996.31, 12.95, function()
        --    print('pressed')
        --end)
    end
end)
