# Pi

Various Dockerized .NET implementations of a Pi calculator.

## Pi.Web

Pi as a .NET Core (3.1) web app.

```
docker run -d -p 8081:80 sixeyed/pi:2002 -m web
```

> Browse to http://localhost:8081?dp=500 and http://localhost:8081/metrics

Or to print out to console:

```
docker run sixeyed/pi:2002

docker run sixeyed/pi:2002 -dp 500
```

Or to write to a file:

```
mkdir pi-core

# linux:
docker run -v "$(pwd)/pi-core:/out" sixeyed/pi:2002 -m file -dp 1000 -o /out/pi.txt

# windows:
docker run -v "$(pwd)/pi-core:C:\out" sixeyed/pi:2002 -m file -dp 1000 -o /out/pi.txt

cat ./pi-core/pi.txt
```

## Pi.NetFx

Pi as a .NET Framework (4.8) console app.

> This only runs as a Windows container, on Windows 10 or Server 2019

```
docker run sixeyed/pi:netfx-2002

docker run sixeyed/pi:netfx-2002 -dp 500
```

Or to write to a file:

```
mkdir pi-netfx

docker run -v "$(pwd)\pi-netfx:C:\out" sixeyed/pi:netfx-2002 -m file -dp 1000 -o C:\out\pi.txt

cat .\pi-netfx\pi.txt
```

## Pi.Wcf

Pi as a REST service, using WCF (.NET Framework 4.8).

> This only runs as a Windows container, on Windows 10 or Server 2019

```
docker run -d -p 8080:80 -p 8088:50505 sixeyed/pi:wcf-2002
```

Check the output and metrics:

```
curl http://localhost:8080/PiService.svc/Pi?dp=800

curl http://localhost:8088/metrics
```
