import java.util.ArrayDeque;
import java.util.Random;
import java.util.Scanner;

public class MapGenerator {
    public static class Pair{
        int i;
        int j;

        public Pair(int i, int j) {
            this.i = i;
            this.j = j;
        }

        @Override
        public boolean equals(Object o) {
            if (this == o) return true;
            if (o == null || getClass() != o.getClass()) return false;
            Pair pair = (Pair) o;
            return i == pair.i && j == pair.j;
        }
    }
    public static boolean checkMap(Pair ini, Pair fin, int[][] map, Pair[][] predecesores, int goalNumber) {
        int maxI = map.length;
        int maxJ = map[0].length;
        boolean[][] visitados = new boolean[maxI][maxJ];
        ArrayDeque<Pair> cola = new ArrayDeque<>();
        cola.add(ini);

        visitados[ini.i][ini.j] = true;

        int[] dI = {-1, 0, 0, 1};
        int[] dJ = {0, -1, 1, 0};

        while (!cola.isEmpty()) {
            Pair aux = cola.poll();

            if (aux.equals(fin)) return true;

            for (int d = 0; d < 4; d++) {
                int ni = aux.i + dI[d];
                int nj = aux.j + dJ[d];

                if (ni >= 0 && ni < maxI && nj >= 0 && nj < maxJ) {
                    if (!visitados[ni][nj] && (map[ni][nj] ==0 || map[ni][nj]==goalNumber)) {
                        visitados[ni][nj] = true;
                        predecesores[ni][nj]= aux;
                        cola.add(new Pair(ni, nj));
                    }
                }
            }
        }
        return false;
    }


    public static int[][] createMap(int maxGoals, int maxBarriers, int maxI, int maxJ, Pair[] goals){
        Random rand = new Random();
        int[][] map = new int[maxI][maxJ];
        int indx = 0;
        int allGoals = maxGoals;
        int allBarriers = maxBarriers;
        while(allGoals>0){
            int i=0;
            int j=0;
            while(i==0 && j==0){
                i = rand.nextInt(maxI);
                j = rand.nextInt(maxJ);
            }
            if(map[i][j]==0){
                map[i][j]=indx+2;
                goals[indx]=new Pair(i,j);
                indx++;
                allGoals--;
            }
        }
        while(allBarriers>0){
            int i=0;
            int j=0;
            while(i==0 && j==0){
                i = rand.nextInt(maxI);
                j = rand.nextInt(maxJ);
            }
            if(map[i][j]==0){
                map[i][j]=1;
                allBarriers--;
            }
        }
        Pair ini = new Pair(0,0);
        StringBuilder sb = new StringBuilder();
        for(int i=0; i<goals.length; i++){
            Pair[][] predecesores = new Pair[maxI][maxJ];
            if(checkMap(ini, goals[i], map, predecesores, i+2)){
                sb.append("Camino para "+(i+2)+":\n");
                sb.append(createSol(ini, goals[i], map, predecesores, maxI, maxJ, i+2));
            }else {
                goals = new Pair[maxGoals];
                return createMap(maxGoals,maxBarriers, maxI, maxJ, goals);
            }
        }
        System.out.println(sb);
        return map;
    }

    public static String createSol(Pair ini, Pair step, int[][] map, Pair[][] predecesores, int maxI, int maxJ, int goalNumber){
        char[][] visualMap = new char[maxI][maxJ];
        int[] dI = {-1, 0, 0, 1};
        int[] dJ = {0, -1, 1, 0};
        char[] directions = { '↓','→','←','↑'};

        for (int i = 0; i < maxI; i++) {
            for (int j = 0; j < maxJ; j++) {
                if (map[i][j] == 1) visualMap[i][j] = '█';  // Montañas
                else if (map[i][j] == goalNumber) visualMap[i][j] = 'X'; // Meta
                else visualMap[i][j] = '.'; // Camino libre
            }
        }

        Pair prevStep = null;
        while (step != null && !step.equals(ini)) {
            if (map[step.i][step.j] == 0) {
                if (prevStep != null) {
                    int dIChange = step.i - prevStep.i;
                    int dJChange = step.j - prevStep.j;

                    for (int d = 0; d < 4; d++) {
                        if (dI[d] == dIChange && dJ[d] == dJChange) {
                            visualMap[step.i][step.j] = directions[d];
                            break;
                        }
                    }
                }
            }
            prevStep = step;
            step = predecesores[step.i][step.j];
        }
        if (visualMap[ini.i][ini.j+1] != '█' && visualMap[ini.i][ini.j+1] != '.') visualMap[ini.i][ini.j] ='→';
        else if (visualMap[ini.i+1][ini.j] != '█' && visualMap[ini.i+1][ini.j] != '.') visualMap[ini.i][ini.j] = '↓';

        StringBuilder sb = new StringBuilder();
        for (int k = 0; k < 8; k++) {
            for (int j = 0; j < 12; j++) {
                sb.append(visualMap[k][j]+ " ");
            }
            sb.append("\n");
        }
        return sb.toString();
    }
    public static void main(String[] args) {
        Scanner sc = new Scanner(System.in);
        int maxI =8;
        int maxJ = 12;
        System.out.println("Introduce el número de metas (1-4):");
        int maxGoals = sc.nextInt();
        System.out.println("Introduce el número de montañas (0-50):");
        int maxBarriers = sc.nextInt();
        Pair[] goals = new Pair[maxGoals];
        int[][] map = createMap(maxGoals, maxBarriers, maxI, maxJ, goals);
        System.out.println("El mapa completo es:");
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 12; j++) {
                System.out.print(map[i][j]+ " ");
            }
            System.out.println();
        }

    }
}