import csv
import time
import random
import json

TopPGList = []
TopSGList = []
TopSFList = []
TopPFList = []
TopCList = []

CList = []
PGList = []
SGList = []
SFList = []
PFList = []

PG_combination = []
SG_combination = []
SF_combination = []
PF_combination = []


used_PG = []
used_SG = []
used_SF = []
used_PF = []
used_ =[]

already_in = 0
class Player(object):
    def __init__(self, name, position, team, price, projected, ppp, id):
        self.name = name
        self.position = position
        self.team = team
        self.price = float(price)
        self.projected = float(projected)
        self.ppp = ppp
        self.id = id

class PG_Starters(object):
    def __init__(self, PG1, PG2):
        self.PG1 = PG1
        self.PG2 = PG2
        self.projected = float(PG1.projected) + float(PG2.projected)
        self.price = float(PG1.price) + float(PG2.price)
        
class SG_Starters(object):
    def __init__(self, SG1, SG2):
        self.SG1 = SG1
        self.SG2 = SG2
        self.projected = float(SG1.projected) + float(SG2.projected)
        self.price = float(SG1.price) + float(SG2.price)

class PF_Starters(object):
    def __init__(self, PF1, PF2):
        self.PF1 = PF1
        self.PF2 = PF2
        self.projected = float(PF1.projected) + float(PF2.projected)
        self.price = float(PF1.price) + float(PF2.price)
        
class SF_Starters(object):
    def __init__(self, SF1, SF2):
        self.SF1 = SF1
        self.SF2 = SF2
        self.projected = float(SF1.projected) + float(SF2.projected)
        self.price = float(SF1.price) + float(SF2.price)
        
class Lineup(object):
    def __init__(self, C, PG, SG, SF, PF):
        self.C = C
        self.PG = PG
        self.SG = SG
        self.SF = SF
        self.PF = PF
        self.total_points = float(C.projected) + float(PG.projected) + float(SG.projected) + float(SF.projected) + float(PF.projected)
        self.total_cost = float(C.price) + float(PG.price) + float(SG.price) + float(SF.price) + float(PF.price)


def clear_lists():
    TopCList.clear()
    TopPGList.clear()
    TopSGList.clear()
    TopSFList.clear()
    TopPFList.clear()
    PG_combination.clear()
    SG_combination.clear()
    SF_combination.clear()
    PF_combination.clear()
    used_PG.clear()
    used_SG.clear()
    used_SF.clear()
    used_PF.clear()


def read_projections_csv(file_path):
    with open(file_path) as csvfile:
        readCSV = csv.reader(csvfile, delimiter=',')
        for row in readCSV:
            if row[0] == 'Name':
                continue
            player = Player(row[0], row[1], row[2], row[3], row[4], row[5], row[2] + row[1])
            if player.position == 'PG':
                TopPGList.append(player)
                PGList.append(player)
            elif player.position == 'SG':
                SGList.append(player)
                TopSGList.append(player)
            elif player.position == 'SF':
                SFList.append(player)
                TopSFList.append(player)
            elif player.position == 'PF':
                PFList.append(player)
                TopPFList.append(player)
            elif player.position == 'C':
                CList.append(player)

def build_combinations():
    for r in range(len(TopPGList) - 1):
        for t in range(r + 1, len(PGList)):
            if(TopPGList[r].name == PGList[t].name):
                continue
            PG_combination.append(PG_Starters(TopPGList[r], PGList[t]))

    for r in range(len(TopSGList) - 1):
        for t in range(r + 1, len(SGList)):
            if(TopSGList[r].name == SGList[t].name):
                continue
            SG_combination.append(SG_Starters(TopSGList[r], SGList[t]))

    for r in range(len(TopSFList) - 1):
        for t in range(r + 1, len(SFList)):
            if(TopSFList[r].name == SFList[t].name):
                continue
            SF_combination.append(SF_Starters(TopSFList[r], SFList[t]))
            
    for r in range(len(TopPFList) - 1):
        for t in range(r + 1, len(PFList)):
            if(TopPFList[r].name == PFList[t].name):
                continue
            PF_combination.append(PF_Starters(TopPFList[r], PFList[t]))


def make_lineup(max_price):
    while True:
        pg = PG_combination[random.randint(0, len(PG_combination) -1)]
        sg = SG_combination[random.randint(0, len(SG_combination) -1)]
        sf = SF_combination[random.randint(0, len(SF_combination) -1)]
        pf = PF_combination[random.randint(0, len(PF_combination) -1)]
        c = CList[random.randint(0, len(CList) - 1)]
        lu = Lineup(c, pg, sg, sf, pf)
        if lu.total_cost <= max_price:
            return lu

def display_top_lineups(lineups, lineup_string):
    print('Top Lineups: ')
    for i in range(len(lineups) -1, -1, -1):
        print('------' + str(i + 1) + '-------')
        print(f'PG1:  {lineups[i].PG.PG1.projected:.5}\t {lineups[i].PG.PG1.name:<20}\t {lineups[i].PG.PG1.team}\t {lineups[i].PG.PG1.price}\t {lineups[i].PG.PG1.ppp:.6}\t')
        print(f'PG2:  {lineups[i].PG.PG2.projected:.5}\t {lineups[i].PG.PG2.name:<20}\t {lineups[i].PG.PG2.team}\t {lineups[i].PG.PG2.price}\t {lineups[i].PG.PG2.ppp:.6}\t')
        print(f'SG1:  {lineups[i].SG.SG1.projected:.5}\t {lineups[i].SG.SG1.name:<20}\t {lineups[i].SG.SG1.team}\t {lineups[i].SG.SG1.price}\t {lineups[i].SG.SG1.ppp:.6}\t')
        print(f'SG2:  {lineups[i].SG.SG2.projected:.5}\t {lineups[i].SG.SG2.name:<20}\t {lineups[i].SG.SG2.team}\t {lineups[i].SG.SG2.price}\t {lineups[i].SG.SG2.ppp:.6}\t')
        print(f'SF1:  {lineups[i].SF.SF1.projected:.5}\t {lineups[i].SF.SF1.name:<20}\t {lineups[i].SF.SF1.team}\t {lineups[i].SF.SF1.price}\t {lineups[i].SF.SF1.ppp:.6}\t')
        print(f'SF2:  {lineups[i].SF.SF2.projected:.5}\t {lineups[i].SF.SF2.name:<20}\t {lineups[i].SF.SF2.team}\t {lineups[i].SF.SF2.price}\t {lineups[i].SF.SF2.ppp:.6}\t')
        print(f'PF1:  {lineups[i].PF.PF1.projected:.5}\t {lineups[i].PF.PF1.name:<20}\t {lineups[i].PF.PF1.team}\t {lineups[i].PF.PF1.price}\t {lineups[i].PF.PF1.ppp:.6}\t')
        print(f'PF2:  {lineups[i].PF.PF2.projected:.5}\t {lineups[i].PF.PF2.name:<20}\t {lineups[i].PF.PF2.team}\t {lineups[i].PF.PF2.price}\t {lineups[i].PF.PF2.ppp:.6}\t')
        print(f'C:    {lineups[i].C.projected:.5}\t {lineups[i].C.name:<20}\t {lineups[i].C.team}\t {lineups[i].C.price}\t {lineups[i].C.ppp:.6}\t')
        print(f'Total Cost: \t\t{lineups[i].total_cost}')
        print(f'Projected Points: \t{lineups[i].total_points:.6}')
        print('\n\n')

def write_lineups(lineups, text_file_to_write, csv_file_to_write, lineup_string):
    with open(text_file_to_write, 'w') as writer:
        writer.write(lineup_string)
        writer.write('Top Lineups: ')
        writer.write('\n')
        for i in range(len(lineups)):
            writer.write('------' + str(i + 1) + '-------')
            writer.write(f'\nPG1:  {lineups[i].PG.PG1.projected:.5}\t {lineups[i].PG.PG1.name:<20}\t {lineups[i].PG.PG1.team}\t {lineups[i].PG.PG1.price}\t {lineups[i].PG.PG1.ppp:.6}\t')
            writer.write(f'\nPG2:  {lineups[i].PG.PG2.projected:.5}\t {lineups[i].PG.PG2.name:<20}\t {lineups[i].PG.PG2.team}\t {lineups[i].PG.PG2.price}\t {lineups[i].PG.PG2.ppp:.6}\t')
            writer.write(f'\nSG1:  {lineups[i].SG.SG1.projected:.5}\t {lineups[i].SG.SG1.name:<20}\t {lineups[i].SG.SG1.team}\t {lineups[i].SG.SG1.price}\t {lineups[i].SG.SG1.ppp:.6}\t')
            writer.write(f'\nSG2:  {lineups[i].SG.SG2.projected:.5}\t {lineups[i].SG.SG2.name:<20}\t {lineups[i].SG.SG2.team}\t {lineups[i].SG.SG2.price}\t {lineups[i].SG.SG2.ppp:.6}\t')
            writer.write(f'\nSF1:  {lineups[i].SF.SF1.projected:.5}\t {lineups[i].SF.SF1.name:<20}\t {lineups[i].SF.SF1.team}\t {lineups[i].SF.SF1.price}\t {lineups[i].SF.SF1.ppp:.6}\t')
            writer.write(f'\nSF2:  {lineups[i].SF.SF2.projected:.5}\t {lineups[i].SF.SF2.name:<20}\t {lineups[i].SF.SF2.team}\t {lineups[i].SF.SF2.price}\t {lineups[i].SF.SF2.ppp:.6}\t')
            writer.write(f'\nPF1:  {lineups[i].PF.PF1.projected:.5}\t {lineups[i].PF.PF1.name:<20}\t {lineups[i].PF.PF1.team}\t {lineups[i].PF.PF1.price}\t {lineups[i].PF.PF1.ppp:.6}\t')
            writer.write(f'\nPF2:  {lineups[i].PF.PF2.projected:.5}\t {lineups[i].PF.PF2.name:<20}\t {lineups[i].PF.PF2.team}\t {lineups[i].PF.PF2.price}\t {lineups[i].PF.PF2.ppp:.6}\t')
            writer.write(f'\nC:    {lineups[i].C.projected:.5}\t {lineups[i].C.name:<20}\t {lineups[i].C.team}\t {lineups[i].C.price}\t {lineups[i].C.ppp:.6}\t')
            writer.write(f'\nTotal Cost: \t\t{lineups[i].total_cost}')
            writer.write(f'\nProjected Points: \t{lineups[i].total_points:.6}')
            writer.write('\n\n')

 #   with open(csv_file_to_write, 'w', newline='') as csvfile:
  #      fieldnames = ['QB','RB1','RB2','WR1', 'WR2', 'WR3', 'TE', 'FLEX', 'DEF', 'DEF Budget', 'Points', 'Total Cost']
   #     writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
    #    writer.writeheader()
     #   for lineup in lineups:
      #      writer.writerow({'QB' : f'{lineup.QB.id}:{lineup.QB.name}','RB1' : f'{lineup.RB.RB1.id}:{lineup.RB.RB1.name}',
       #                         'RB2': f'{lineup.RB.RB2.id}:{lineup.RB.RB2.name}', 'WR1' : f'{lineup.WR.WR1.id}:{lineup.WR.WR1.name}',
        #                        'WR2' : f'{lineup.WR.WR2.id}:{lineup.WR.WR2.name}', 'WR3' : f'{lineup.WR.WR3.id}:{lineup.WR.WR3.name}', 
         #                       'TE' : f'{lineup.TE.id}:{lineup.TE.name}', 'FLEX' : f'{lineup.FLEX.id}:{lineup.FLEX.name}', 'DEF':'',
          #                      'DEF Budget' : str(60000 - lineup.total_cost), 'Points' : lineup.total_points, 'Total Cost' : lineup.total_cost})

def write_lineups_json(lineups, text_file_to_write):
    with open(text_file_to_write, 'w') as outfile:
        json.dump(lineups, outfile)

def sort_lineups(lineups, keep):
    rtn_lineups = []
    used_lineups = []
    global already_in
    lineups = sorted(lineups,key=lambda x: x.total_points, reverse=True)
    for lineup in lineups:
        lineup_string = ''.join(sorted(lineup.C.name + lineup.PG.PG1.name + lineup.PG.PG2.name + lineup.SG.SG1.name + lineup.SG.SG2.name + lineup.SF.SF1.name + lineup.SF.SF2.name + lineup.PF.PF1.name + lineup.PF.PF2.name + lineup.C.id + lineup.PG.PG1.id + lineup.PG.PG2.id + lineup.SG.SG1.id + lineup.SG.SG2.id + lineup.SF.SF1.id + lineup.SF.SF2.id + lineup.PF.PF1.id + lineup.PF.PF2.name))
        if lineup_string not in used_lineups:
            rtn_lineups.append(lineup)
            used_lineups.append(lineup_string)
        else:
            print(f'Already in')
            already_in = already_in + 1
        if len(rtn_lineups) >= keep:
            break

    return rtn_lineups

def run_generator(config):
    loop_num = 0
    display_num = 0
    
    read_projections_csv(config['ReadFile'])
    top_lineups = []
    total_loops = 0
    initial = True
    while True:
        print('optimizing lineups...')
        if not initial:
            optimize_top_lineups(top_lineups)
        initial = False
        writes_til_read_new = config['WTNR']
        build_combinations()
        print(f'PGs: {len(TopPGList)}\nSGs: {len(TopSGList)}\nSFs: {len(TopSFList)}\nPFs: {len(TopPFList)}\nCs: {len(CList)}\nPG Combos: {len(PG_combination)}\nSG Combos: {len(SG_combination)}\nSF Combos: {len(SF_combination)}\nPF Combos: {len(PF_combination)}\n')
        while writes_til_read_new != 0:
            lineup = make_lineup(config['MaxPrice'])
            top_lineups.append(lineup)
            loop_num = loop_num + 1
            if loop_num == 1000000:
                print(f'sorting {len(top_lineups)}')
                top_lineups = sort_lineups(top_lineups, config['Lineups'])
                print(f'done {len(top_lineups)}')
                total_loops = total_loops + 1
                loop_num = 0
                display_num = display_num + 1
                display_totals(total_loops, display_num, writes_til_read_new)
                if(display_num == 10):
                    player_list = build_lineup_player_string(top_lineups)
                    print(player_list)
                    display_top_lineups(top_lineups, player_list)
                    print(f'PGs: {len(TopPGList)}\nSGs: {len(TopSGList)}\nSFs: {len(TopSFList)}\nPFs: {len(TopPFList)}\nCs: {len(CList)}\nPG Combos: {len(PG_combination)}\nSG Combos: {len(SG_combination)}\nSF Combos: {len(SF_combination)}\nPF Combos: {len(PF_combination)}\n')
                    print(f'{already_in} duplicated lineups')
                    display_num = 0
                    writes_til_read_new = writes_til_read_new - 1
                    print('writing lineups')
                    write_lineups(top_lineups, config['LineupOutput'], "", player_list)
                    print(f'----------WTNR {writes_til_read_new}------------')
                    # print('writing json')
                    # write_lineups_json(top_lineups, 'top_lineups_week_03_json.txt')
                    print('done')
                    time.sleep(2)


def display_totals(loops, display, wtrn):
    print(f'\nLineups Generated: {loops} Million')
    print(f'Display Loop: {display}')
    print(f'Writes til new read: {wtrn}\n\n')

def optimize_top_lineups(top_lineups):
    print(f'before PGs: {len(TopPGList)}\nSGs: {len(TopSGList)}\nSFs: {len(TopSFList)}\nPFs: {len(TopPFList)}\nCs: {len(CList)}\nPG Combos: {len(PG_combination)}\nSG Combos: {len(SG_combination)}\nSF Combos: {len(SF_combination)}\nPF Combos: {len(PF_combination)}\n')
    clear_lists()
    for lineup in top_lineups:
        if lineup.PG.PG1.name not in used_PG:
            TopPGList.append(lineup.PG.PG1)
            used_PG.append(lineup.PG.PG1.name)
        if lineup.PG.PG2.name not in used_PG:
            TopPGList.append(lineup.PG.PG2)
            used_PG.append(lineup.PG.PG2.name)
        if lineup.SG.SG1.name not in used_SG:
            TopSGList.append(lineup.SG.SG1)
            used_SG.append(lineup.SG.SG1.name)
        if lineup.SG.SG2.name not in used_SG:
            TopSGList.append(lineup.SG.SG2)
            used_SG.append(lineup.SG.SG2.name)
        if lineup.SF.SF1.name not in used_SF:
            TopSFList.append(lineup.SF.SF1)
            used_SF.append(lineup.SF.SF1.name)
        if lineup.SF.SF2.name not in used_SF:
            TopSFList.append(lineup.SF.SF2)
            used_SF.append(lineup.SF.SF2.name)
        if lineup.PF.PF1.name not in used_PF:
            TopPFList.append(lineup.PF.PF1)
            used_PF.append(lineup.PF.PF1.name)
        if lineup.PF.PF2.name not in used_PF:
            TopPFList.append(lineup.PF.PF2)
            used_PF.append(lineup.PF.PF2.name)
    print(f'after PGs: {len(TopPGList)}\nSGs: {len(TopSGList)}\nSFs: {len(TopSFList)}\nPFs: {len(TopPFList)}\nCs: {len(CList)}\nPG Combos: {len(PG_combination)}\nSG Combos: {len(SG_combination)}\nSF Combos: {len(SF_combination)}\nPF Combos: {len(PF_combination)}\n')


def build_lineup_player_string(lineups):
    pg_used = []
    sg_used = []
    sf_used = []
    pf_used = []
    c_used = []
    pg_string = ''
    sg_string = ''
    sf_string = ''
    pf_string = ''
    c_string = ''
    for lineup in lineups:
        if lineup.PG.PG1.name not in pg_used:
            pg_string = pg_string  + lineup.PG.PG1.name + ', '
            pg_used.append(lineup.PG.PG1.name)
        if lineup.PG.PG2.name not in pg_used:
            pg_string = pg_string  + lineup.PG.PG2.name + ', '
            pg_used.append(lineup.PG.PG2.name)
        if lineup.SG.SG1.name not in sg_used:
            sg_string = sg_string  + lineup.SG.SG1.name + ', '
            sg_used.append(lineup.SG.SG1.name)
        if lineup.SG.SG2.name not in sg_used:
            sg_string = sg_string  + lineup.SG.SG2.name + ', '
            sg_used.append(lineup.SG.SG2.name)
        if lineup.SF.SF1.name not in sf_used:
            sf_string = sf_string  + lineup.SF.SF1.name + ', '
            sf_used.append(lineup.SF.SF1.name)
        if lineup.SF.SF2.name not in sf_used:
            sf_string = sf_string  + lineup.SF.SF2.name + ', '
            sf_used.append(lineup.SF.SF2.name)
        if lineup.PF.PF1.name not in pf_used:
            pf_string = pf_string  + lineup.PF.PF1.name + ', '
            pf_used.append(lineup.PF.PF1.name)
        if lineup.PF.PF2.name not in pf_used:
            pf_string = pf_string  + lineup.PF.PF2.name + ', '
            pf_used.append(lineup.PF.PF2.name)
        if lineup.C.name not in c_used:
            c_string = c_string  + lineup.C.name + ', '
            c_used.append(lineup.C.name)

    pg_string = pg_string[0:len(pg_string) - 2]
    sg_string = sg_string[0:len(sg_string) - 2]
    sf_string = sf_string[0:len(sf_string) - 2]
    pf_string = pf_string[0:len(pf_string) - 2]
    c_string = c_string[0:len(c_string) - 2]
    rtn_string = f'PG: {pg_string}\nSG: {sg_string}\nSF: {sf_string}\nPF: {pf_string}\nC:  {c_string}'
    return rtn_string
    



        

def read_config(path):
    config = {}
    with open(path) as txtfile:
        reader = txtfile.readlines()
        for read in reader:
            line = read.rstrip().split('::')
            if line[0] == 'Lineups':
                config['Lineups'] = float(line[1])
            elif 'Writes til' in line[0]:
                config['WTNR'] = float(line[1])
            elif 'File to read' in line[0]:
                config['ReadFile'] = line[1]
            elif 'Top Lineup output path' in line[0]:
                config['LineupOutput'] = line[1]
            elif 'Top Lineup CSV output path' in line[0]:
                config['CSVOutput'] = line[1]
            elif 'Max Price' in line[0]:
                config["MaxPrice"] = float(line[1])
    
    print(config)
    return config
 
def main():
    config = read_config('C:\\Users\\15133\\Documents\\dfs\\Basketball\\dfs-nba-config.txt')
    run_generator(config)

if __name__ == '__main__':
    main()
