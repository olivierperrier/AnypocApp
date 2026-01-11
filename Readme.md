  To deploy updates in the future:
  # 1. Build for ARM64
  dotnet publish -c Release -r linux-arm64 --self-contained true -o ./publish

  # 2. Transfer to IMX93
  scp ./publish/AnypocApp root@192.168.1.29:/root/

  # 3. Run on IMX93
  ssh root@192.168.1.29 "export DISPLAY=:0 && /root/AnypocApp"

